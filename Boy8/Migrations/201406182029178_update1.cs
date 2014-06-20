namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stories",
                c => new
                    {
                        StoryId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Body = c.String(nullable: false),
                        Abstract = c.String(),
                        Picture = c.String(),
                        Vedio = c.String(),
                        OtherResources = c.String(),
                        StoryCreatedTime = c.DateTime(nullable: false),
                        SyncTime = c.DateTime(nullable: false),
                        Source = c.String(nullable: false),
                        SyncAppID = c.String(),
                        SyncAccount = c.String(),
                        SyncComment = c.String(),
                        SyncOther = c.String(),
                        Email = c.String(),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StoryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stories");
        }
    }
}
