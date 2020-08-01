using S3.EC2.Integration.Entities;
using System;
using System.Collections.Generic;
using System.Text;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;


namespace S3.EC2.Integration.S3
{
    public class S3Process
    {
        public bool ProcessBuckets(Configurations configs)
        {
            try
            {
                foreach(Account account in configs.Accounts)
                {
                    foreach(Bucket bucket in account.Buckets)
                    {
                        ProcessS3ToEC2(bucket.S3ToEC2s, bucket.S3Bucket, bucket.S3Region, account.AccessID, account.SecretKey);
                        ProcessEC2ToS3(bucket.EC2ToS3s, bucket.S3Bucket, bucket.S3Region, account.AccessID, account.SecretKey);
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }

        private bool ProcessS3ToEC2(List<SourceDestFolder> folderList, string bucket, string region, string accessid, string secretkey)
        {
            RegionEndpoint bucketRegion;
            IAmazonS3 client;
            try
            {
                bucketRegion = RegionEndpoint.GetBySystemName(region);
                if (string.IsNullOrEmpty(accessid) || string.IsNullOrEmpty(secretkey))
                {
                    client = new AmazonS3Client(bucketRegion);
                }
                else
                {
                    client = new AmazonS3Client(accessid, secretkey, bucketRegion);
                }

                S3toEC2 s3toec2 = new S3toEC2();
                foreach (SourceDestFolder folder in folderList)
                {
                    s3toec2.GetObjects(client,bucket,folder.S3Folder,folder.EC2folder, folder.FileType).Wait();
                }
                client.Dispose();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool ProcessEC2ToS3(List<SourceDestFolder> folderList, string bucket, string region, string accessid, string secretkey)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
