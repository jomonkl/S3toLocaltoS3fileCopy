using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace S3.EC2.Integration.Entities
{
    [Serializable]
    public class SourceDestFolder
    {
        [JsonProperty("S3FolderPath")]
        public string S3Folder { get; set; }
        [JsonProperty("Ec2FilePath")]
        public string EC2folder { get; set; }
        [JsonProperty("FileType")]
        public string FileType { get; set; }
    }
}
