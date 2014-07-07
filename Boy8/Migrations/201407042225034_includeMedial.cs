namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class includeMedial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MediaAssets",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MediaJobs",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MediaJobs");
            DropTable("dbo.MediaAssets");
        }
    }
}
