namespace OnlineLib.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LoanActivities", "LoanData", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.LoanActivities", "ReturnedData", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.LoanActivities", "ScheduledReturnData", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LoanActivities", "ScheduledReturnData", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LoanActivities", "ReturnedData", c => c.DateTime());
            AlterColumn("dbo.LoanActivities", "LoanData", c => c.DateTime(nullable: false));
        }
    }
}
