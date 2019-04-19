namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4197 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "IsManualyActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "IsManualyActive");
        }
    }
}
