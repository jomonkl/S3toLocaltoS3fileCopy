using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using S3.EC2.Integration.Entities;

using System.IO;
using Amazon.Util.Internal.PlatformServices;
using S3.EC2.Integration.S3;

namespace S3.EC2.Integration
{
    class S3EC2Integration
    {
        static void Main(string[] args)
        {
            try
            {
                string Configfile = AppConfigurations.ConfigFileName;
                Configurations configs = new JSONSerializer().ReadJSON<Configurations>(Configfile);

                S3Process s3Process = new S3Process();
                if (!configs.Equals(null) )
                {
                    s3Process.ProcessBuckets(configs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Do something here
            }
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
