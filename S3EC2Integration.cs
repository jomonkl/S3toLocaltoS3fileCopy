using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using S3.EC2.Integration.Entities;

using System.IO;
using Amazon.Util.Internal.PlatformServices;

namespace S3.EC2.Integration
{
    class S3EC2Integration
    {
        static void Main(string[] args)
        {

            string Configfile = System.Reflection.Assembly.GetExecutingAssembly().Location + AppConfigurations.ConfigFileName;
            Configurations config = new JSONSerializer().ReadJSON<Configurations>(Configfile);

            //Amazon.S3.

        }
    }

    public class JSONSerializer
    {
        public T  ReadJSON<T>(string FilePath) where T: class
        {
            StreamReader reader = new StreamReader(FilePath);
            
            JsonSerializer serializer = new JsonSerializer();
            return (T)serializer.Deserialize(reader, typeof(T));
        }
    }
}
