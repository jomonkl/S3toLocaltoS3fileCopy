using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace S3.EC2.Integration.Entities
{
    [Serializable]
    public class Bucket
    {
        [JsonProperty("S3Bucket")]
        public string S3Bucket { get; set; }
        [JsonProperty("S3Region")]
        public string S3Region { get; set; }

        [JsonProperty("S3ToEC2s")]
        public List<SourceDestFolder> S3ToEC2s { get; set; }

        [JsonProperty("EC2ToS3s")]
        public List<SourceDestFolder> EC2ToS3s { get; set; }

    }
}
