namespace Boy7.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Boy7.Models.Boy7Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Boy7.Models.Boy7Context context)
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

            //by Zhe
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<Boy7.Models.Boy7Context, Configuration>());

            //using (var db = new Boy7.Models.Boy7Context())
            //{
            //    db.Stories.Add(new Boy7.Models.Story { Title = "Another T ", Body = "Body PH"});
            //    db.SaveChanges();

            //} 
        }
    }
}
