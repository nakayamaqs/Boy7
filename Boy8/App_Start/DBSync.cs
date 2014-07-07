using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Boy8.App_Start
{
    public class DBSync
    {
        public static void SyncDBChangesToAzure()
        {
            if (bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"]))
            {
                //    var configuration = new MvcWebRole.Migrations.Configuration();
                //var migrator = new DbMigrator(configuration);
                //migrator.Update();
            }

        }
    }
}