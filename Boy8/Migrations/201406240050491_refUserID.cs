namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refUserID : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ResourceId = c.Int(nullable: false, identity: true),
                        Res_URL = c.String(),
                        Res_Type = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Story_StoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ResourceId)
                .ForeignKey("dbo.Stories", t => t.Story_StoryId)
                .Index(t => t.Story_StoryId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 500),
                        Status = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        Boy7UserId = c.String(maxLength: 128),
                        Resource_ResourceId = c.Int(),
                        Story_StoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Boy7UserId)
                .ForeignKey("dbo.Resources", t => t.Resource_ResourceId)
                .ForeignKey("dbo.Stories", t => t.Story_StoryId)
                .Index(t => t.Boy7UserId)
                .Index(t => t.Resource_ResourceId)
                .Index(t => t.Story_StoryId);
            
            CreateTable(
                "dbo.SyncSources",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Stories", "StoryUpdatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Stories", "SyncSourceID", c => c.Int());
            CreateIndex("dbo.Stories", "SyncSourceID");
            AddForeignKey("dbo.Stories", "SyncSourceID", "dbo.SyncSources", "ID");
            DropColumn("dbo.Stories", "Picture");
            DropColumn("dbo.Stories", "Vedio");
            DropColumn("dbo.Stories", "OtherResources");
            DropColumn("dbo.Stories", "Source");
            DropColumn("dbo.Stories", "SyncAppID");
            DropColumn("dbo.Stories", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stories", "Email", c => c.String());
            AddColumn("dbo.Stories", "SyncAppID", c => c.String());
            AddColumn("dbo.Stories", "Source", c => c.String(nullable: false));
            AddColumn("dbo.Stories", "OtherResources", c => c.String());
            AddColumn("dbo.Stories", "Vedio", c => c.String());
            AddColumn("dbo.Stories", "Picture", c => c.String());
            DropForeignKey("dbo.Stories", "SyncSourceID", "dbo.SyncSources");
            DropForeignKey("dbo.Resources", "Story_StoryId", "dbo.Stories");
            DropForeignKey("dbo.Comments", "Story_StoryId", "dbo.Stories");
            DropForeignKey("dbo.Comments", "Resource_ResourceId", "dbo.Resources");
            DropForeignKey("dbo.Comments", "Boy7UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Stories", new[] { "SyncSourceID" });
            DropIndex("dbo.Comments", new[] { "Story_StoryId" });
            DropIndex("dbo.Comments", new[] { "Resource_ResourceId" });
            DropIndex("dbo.Comments", new[] { "Boy7UserId" });
            DropIndex("dbo.Resources", new[] { "Story_StoryId" });
            DropColumn("dbo.Stories", "SyncSourceID");
            DropColumn("dbo.Stories", "StoryUpdatedTime");
            DropTable("dbo.SyncSources");
            DropTable("dbo.Comments");
            DropTable("dbo.Resources");
        }
    }
}
