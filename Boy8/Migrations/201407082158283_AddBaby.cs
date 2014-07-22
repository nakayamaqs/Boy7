namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBaby : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Babies",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Male = c.Boolean(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        Thumbnail_ResourceId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Resources", t => t.Thumbnail_ResourceId)
                .Index(t => t.Thumbnail_ResourceId);
            
            AddColumn("dbo.AspNetUsers", "HasBabyLinked", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "BabyID", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "BabyID");
            AddForeignKey("dbo.AspNetUsers", "BabyID", "dbo.Babies", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Babies", "Thumbnail_ResourceId", "dbo.Resources");
            DropForeignKey("dbo.AspNetUsers", "BabyID", "dbo.Babies");
            DropIndex("dbo.AspNetUsers", new[] { "BabyID" });
            DropIndex("dbo.Babies", new[] { "Thumbnail_ResourceId" });
            DropColumn("dbo.AspNetUsers", "BabyID");
            DropColumn("dbo.AspNetUsers", "HasBabyLinked");
            DropTable("dbo.Babies");
        }
    }
}
