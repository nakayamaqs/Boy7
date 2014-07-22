namespace Boy8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentBabyRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "BabyID", "dbo.Babies");
            DropIndex("dbo.AspNetUsers", new[] { "BabyID" });
            CreateTable(
                "dbo.Boy7UserBaby",
                c => new
                    {
                        Boy7User_Id = c.String(nullable: false, maxLength: 128),
                        Baby_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Boy7User_Id, t.Baby_ID })
                .ForeignKey("dbo.AspNetUsers", t => t.Boy7User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Babies", t => t.Baby_ID, cascadeDelete: true)
                .Index(t => t.Boy7User_Id)
                .Index(t => t.Baby_ID);
            
            DropColumn("dbo.AspNetUsers", "BabyID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "BabyID", c => c.Guid());
            DropForeignKey("dbo.Boy7UserBaby", "Baby_ID", "dbo.Babies");
            DropForeignKey("dbo.Boy7UserBaby", "Boy7User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Boy7UserBaby", new[] { "Baby_ID" });
            DropIndex("dbo.Boy7UserBaby", new[] { "Boy7User_Id" });
            DropTable("dbo.Boy7UserBaby");
            CreateIndex("dbo.AspNetUsers", "BabyID");
            AddForeignKey("dbo.AspNetUsers", "BabyID", "dbo.Babies", "ID");
        }
    }
}
