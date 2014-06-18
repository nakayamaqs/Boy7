namespace Boy7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStoryV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stories", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stories", "Rating");
        }
    }
}
