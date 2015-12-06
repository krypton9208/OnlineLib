namespace OnlineLib.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Third : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserRoles", "Library", c => c.Int());
            CreateIndex("dbo.AspNetUserRoles", "Library");
            AddForeignKey("dbo.AspNetUserRoles", "Library", "dbo.Library", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "Library", "dbo.Library");
            DropIndex("dbo.AspNetUserRoles", new[] { "Library" });
            DropColumn("dbo.AspNetUserRoles", "Library");
        }
    }
}
