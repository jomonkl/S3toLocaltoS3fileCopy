using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace S3.EC2.Integration.Entities
{
    [Serializable]
    [JsonObject(Id = "Configurations")]
    public class Configurations
    {
        public List<Account> Accounts { get; set; }
    }
}
