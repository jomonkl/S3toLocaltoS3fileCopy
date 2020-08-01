using System;
using System.Collections.Generic;
using System.IO;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;
using S3.EC2.Integration.Entities;

namespace S3.EC2.Integration.S3
{
    public class S3toEC2
    {

        private string bucketName; // = "*** bucket name ***";
        private string keyName;// = "*** folder path ***";
        private string localFolderPath;// = "";
        private string regionEndPoint;// = "";

        private IAmazonS3 client;

        public async Task GetObjects(IAmazonS3 iclient, string bucket,string s3folder, string localfolder, string filetype)
        {
            client = iclient;
            bucketName = bucket;
            keyName = s3folder;
            localFolderPath = localfolder;

            await ReadObjectDataAsync();
        }

        private async Task ReadObjectDataAsync()
        {
            string responseBody = "";
            try
            {
                ListObjectsV2Request listRequest = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    Prefix = keyName,
                    Delimiter = "/"
                };

                ListObjectsV2Response listResponse;

                do
                {
                    listResponse = await client.ListObjectsV2Async(listRequest);
                    foreach (S3Object entry in listResponse.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                                               

                        GetObjectRequest objRequest = new GetObjectRequest
                        {
                            BucketName = entry.BucketName,
                            Key = entry.Key
                        };
                        using (GetObjectResponse objResponse = await client.GetObjectAsync(objRequest))
                        using (Stream responseStream = objResponse.ResponseStream)
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string title = objResponse.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                            string contentType = objResponse.Headers["Content-Type"];
                            Console.WriteLine("Object metadata, Title: {0}", title);
                            Console.WriteLine("Content type: {0}", contentType);

                            responseBody = reader.ReadToEnd(); // Now you process the response body.
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
