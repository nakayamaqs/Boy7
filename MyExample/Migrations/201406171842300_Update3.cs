namespace Boy7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stories", "Title", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Stories", "Source", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stories", "Source", c => c.String());
            AlterColumn("dbo.Stories", "Title", c => c.String(maxLength: 200));
        }
    }
}
