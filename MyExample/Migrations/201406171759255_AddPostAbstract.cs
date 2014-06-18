namespace Boy7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostAbstract : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stories", "Abstract", c => c.String());
            Sql("UPDATE dbo.Stories SET Abstract = LEFT(Body, 100) WHERE Abstract IS NULL"); 
            AlterColumn("dbo.Stories", "Title", c => c.String(nullable: true, maxLength: 200));
            AlterColumn("dbo.Stories", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stories", "Body", c => c.String());
            AlterColumn("dbo.Stories", "Title", c => c.String());
            DropColumn("dbo.Stories", "Abstract");
        }
    }
}
