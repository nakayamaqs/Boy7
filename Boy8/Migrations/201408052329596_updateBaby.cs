namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBaby : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Babies",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Male = c.Int(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        Thumbnail_ResourceId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Resources", t => t.Thumbnail_ResourceId)
                .Index(t => t.Thumbnail_ResourceId);
            
            CreateTable(
                "dbo.Boy7UserBaby",
                c => new
                    {
                        Boy7User_Id = c.String(nullable: false, maxLength: 128),
                        Baby_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Boy7User_Id, t.Baby_ID })
                .ForeignKey("dbo.AspNetUsers", t => t.Boy7User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Babies", t => t.Baby_ID, cascadeDelete: true)
                .Index(t => t.Boy7User_Id)
                .Index(t => t.Baby_ID);
            
            AddColumn("dbo.AspNetUsers", "HasBabyLinked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Babies", "Thumbnail_ResourceId", "dbo.Resources");
            DropForeignKey("dbo.Boy7UserBaby", "Baby_ID", "dbo.Babies");
            DropForeignKey("dbo.Boy7UserBaby", "Boy7User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Boy7UserBaby", new[] { "Baby_ID" });
            DropIndex("dbo.Boy7UserBaby", new[] { "Boy7User_Id" });
            DropIndex("dbo.Babies", new[] { "Thumbnail_ResourceId" });
            DropColumn("dbo.AspNetUsers", "HasBabyLinked");
            DropTable("dbo.Boy7UserBaby");
            DropTable("dbo.Babies");
        }
    }
}
