namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stories", "StoryCreatedOrSyncTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Stories", "SyncTime");
            DropColumn("dbo.Stories", "StoryCreatedTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stories", "StoryCreatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Stories", "SyncTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Stories", "StoryCreatedOrSyncTime");
        }
    }
}
