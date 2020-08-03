using System;
using System.Collections.Generic;
using System.IO;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;
using S3.EC2.Integration.Entities;
using System.Text.RegularExpressions;

namespace S3.EC2.Integration.S3
{
    public class S3toEC2
    {

        private string bucketName; // = "*** bucket name ***";
        private string keyName;// = "*** folder path ***";
        private string localFolderPath;// = "";
        private string fileName;
        private string fileType;
        //private string regionEndPoint;// = "";

        private IAmazonS3 client;

        public async Task GetObjects(IAmazonS3 iclient, string bucket,string s3folder, string localfolder, string filetype)
        {
            client = iclient;
            bucketName = bucket;
            keyName = s3folder;
            localFolderPath = localfolder;
            fileType = filetype;

            await ReadObjectDataAsync();
        }

        private async Task ReadObjectDataAsync()
        {
            try
            {
                //S3ResourceId path = S3ResourceId.fromUri("s3://" + bucketName + "/" + keyName + "\\" + fileType);

                //Match match = Regex.Match("hellp.txt", "*.txt");

                ListObjectsV2Request listRequest = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    Prefix = keyName,
                    Delimiter = "/"
                };

                Console.WriteLine("Going to:" + keyName);
                ListObjectsV2Response listResponse;

                do
                {
                    listResponse = await client.ListObjectsV2Async(listRequest);
                    
                    foreach (S3Object entry in listResponse.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);

                        string[] keySplits;
                        keySplits = entry.Key.Split("/");

                        if (keySplits.Length > 0)
                        { 
                            fileName = keySplits[keySplits.Length - 1];
                        }

                        string fileTypeExp = fileType.Replace("*", "[^*]+");

                        if (Regex.IsMatch(fileName, fileTypeExp, RegexOptions.IgnoreCase))
                        { 
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                GetObjectRequest objRequest = new GetObjectRequest
                                {
                                    BucketName = entry.BucketName,
                                    Key = entry.Key
                                };
                                using (GetObjectResponse objResponse = await client.GetObjectAsync(objRequest))
                                using (Stream responseStream = objResponse.ResponseStream)

                                using (FileStream fs = new FileStream(localFolderPath + "\\" + fileName, FileMode.Create, FileAccess.Write))
                                {
                                    byte[] data = new byte[32768];
                                    int bytesRead = 0;
                                    do
                                    {
                                        bytesRead = responseStream.Read(data, 0, data.Length);
                                        fs.Write(data, 0, bytesRead);
                                    }
                                    while (bytesRead > 0);
                                    fs.Flush();

                                    fs.Close();
                                    responseStream.Close();
                                }
                                
                                var deleteObjectRequest = new DeleteObjectRequest
                                {
                                    BucketName = bucketName,
                                    Key = entry.Key
                                };

                                Console.WriteLine("Deleting an object");
                                await client.DeleteObjectAsync(deleteObjectRequest);
                            }
                        }
                    }
                    
                    listRequest.ContinuationToken = listResponse.NextContinuationToken;
                } while (listResponse.IsTruncated);
            }
            catch (AmazonS3Exception e)
            {
                // If bucket or object does not exist
                Console.WriteLine("Error encountered ***. Message:'{0}' when reading object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when reading object", e.Message);
            }
        }
    }
}
