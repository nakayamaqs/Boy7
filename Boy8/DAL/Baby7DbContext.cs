using Boy8.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Boy8.DAL
{
    public class Baby7DbContext : IdentityDbContext<Boy7User>
    {
        public Baby7DbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static Baby7DbContext Create()
        {
            return new Baby7DbContext();
        }

        public System.Data.Entity.DbSet<Boy8.Models.Story> Stories { get; set; }
        public System.Data.Entity.DbSet<Boy8.Models.Resource> Resources { get; set; }

        public System.Data.Entity.DbSet<Boy8.Models.MediaAsset> MediaAsset { get; set; }

        public System.Data.Entity.DbSet<Boy8.Models.MediaJob> MediaJob { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
    }

}