namespace OnlineLib.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Adress_Id", "dbo.Addresses");
            DropIndex("dbo.AspNetUsers", new[] { "Adress_Id" });
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Adress_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "AdresId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AspNetUsers", "Adress_Id");
            AddForeignKey("dbo.AspNetUsers", "Adress_Id", "dbo.Addresses", "Id");
        }
    }
}
