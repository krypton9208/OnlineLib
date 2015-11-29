namespace OnlineLib.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Libraries", "Photo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Libraries", "Photo", c => c.Byte(nullable: false));
        }
    }
}
