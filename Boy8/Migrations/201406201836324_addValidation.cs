namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stories", "Abstract", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stories", "Abstract", c => c.String());
        }
    }
}
