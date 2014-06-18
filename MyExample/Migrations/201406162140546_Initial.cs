namespace Boy7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stories",
                c => new
                    {
                        StoryId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        Picture = c.String(),
                        Vedio = c.String(),
                        OtherResources = c.String(),
                        StoryCreatedTime = c.DateTime(nullable: false),
                        SyncTime = c.DateTime(nullable: false),
                        Source = c.String(),
                        SyncAppID = c.String(),
                        SyncAccount = c.String(),
                        SyncComment = c.String(),
                        SyncOther = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.StoryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stories");
        }
    }
}
