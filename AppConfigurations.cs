using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace S3.EC2.Integration
{
    class AppConfigurations
    {
        public static string ConfigFileName
        {
            get { return ConfigurationManager.AppSettings["ConfigFileName"]; }
        }
    }
}
