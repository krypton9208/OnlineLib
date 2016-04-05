namespace OnlineLib.Models.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ShortGuid: ", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "ShortGuid: ");
        }
    }
}