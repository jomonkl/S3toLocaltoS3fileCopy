using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace S3.EC2.Integration.Entities
{
    [Serializable]
    public class Configurations
    {
        public List<S3toEC2Config> S3toEC2Configs { get; set; }
        public List<EC2toS3Config> EC2ToS3Configs { get; set; }
    }
}
