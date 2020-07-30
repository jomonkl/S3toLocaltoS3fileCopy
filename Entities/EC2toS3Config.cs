using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace S3.EC2.Integration.Entities
{
    [Serializable]
    public class EC2toS3Config
    {
        public string S3Bucket { get; set; } 
        public string S3EndPoint { get; set; } 

        public string FileType { get; set; } 
        public string Ec2FilePath { get; set; } 
    }
}
