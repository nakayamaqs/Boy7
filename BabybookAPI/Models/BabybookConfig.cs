using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BabybookAPI.Models
{
    public class BabybookConfig
    {
        public static string ContainerName
        {
            get
            {
                return ConfigurationManager.AppSettings["ContainerName"];
            }
        }
    }
}