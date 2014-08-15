using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Boy8.Models
{
    public class Boy7Config
    {
        public static string ContainerName {
            get
            {
                return ConfigurationManager.AppSettings["ContainerName"];
            }
        }
    }
}