using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace S3.EC2.Integration.Entities
{
    [Serializable]
    public class Account
    {
        //If running from EC2 which has access to the bucket, probably no need to provide the access id and secret key
        [JsonProperty("AccessID")]
        public string AccessID { get; set; }

        [JsonProperty("SecretKey")]
        public string SecretKey { get; set; }

        [JsonProperty("Buckets")]
        public List<Bucket> Buckets { get; set; }
    }
}
