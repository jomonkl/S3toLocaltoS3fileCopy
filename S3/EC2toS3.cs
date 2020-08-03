using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using System.IO;

namespace S3.EC2.Integration.S3
{
    class EC2toS3
    {
        private string bucketName; // = "*** bucket name ***";
        private string keyName;// = "*** folder path ***";
        private string localFolderPath;// = "";
        //private string fileName;
        private string fileType;

        private static IAmazonS3 client;

        public async Task PutObjects(IAmazonS3 iclient, string bucket, string s3folder, string localfolder, string filetype)
        {
            client = iclient;
            bucketName = bucket;
            keyName = s3folder;
            localFolderPath = localfolder;
            fileType = filetype;

            await WriteObjectDataAsync();

        }

        private async Task WriteObjectDataAsync()
        {
            try
            {
                if (Directory.Exists(localFolderPath))
                {
                    string[] fileEntries = Directory.GetFiles(localFolderPath, fileType);
                    foreach (string fileName in fileEntries)
                    {
                        PutObjectRequest putRequest = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = keyName + Path.GetFileName(fileName),
                            FilePath = fileName
                        };

                        PutObjectResponse response = await client.PutObjectAsync(putRequest);

                        File.Delete(fileName);
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }
    }
}
