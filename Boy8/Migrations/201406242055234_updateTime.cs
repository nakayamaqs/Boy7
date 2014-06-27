namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stories", "StoryUpdatedTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stories", "StoryUpdatedTime", c => c.DateTime(nullable: false));
        }
    }
}
