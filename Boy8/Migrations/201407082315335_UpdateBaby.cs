namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBaby : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Babies", "Male", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Babies", "Male", c => c.Boolean(nullable: false));
        }
    }
}
