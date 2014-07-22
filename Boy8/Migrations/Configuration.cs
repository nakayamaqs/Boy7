using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Boy8.Models;
using Boy8.DAL;

namespace Boy8.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Boy8.DAL.Baby7DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Baby7DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            AddUserAndRole(context); //test data.
        }

        bool AddUserAndRole(Baby7DbContext context)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("parent"));

            var um = new UserManager<Boy7User>(new UserStore<Boy7User>(context));
            var user = new Boy7User()
            {
                UserName = "哲",
                Email = "zhe1@boywecare.com"
            };

            ir = um.Create(user, "P_assw0rd1");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "parent");
            return ir.Succeeded;
        }
    }
}
