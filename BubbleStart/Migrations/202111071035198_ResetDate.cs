namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "ResetDate", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "ResetDate");
        }
    }
}
