namespace Boy7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStoryV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stories", "Title", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stories", "Title", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
