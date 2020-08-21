namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class summer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BubbleCustomers", "ForceDisable", c => c.Int(nullable: false));
            DropColumn("dbo.BubbleCustomers", "IsManualyActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BubbleCustomers", "IsManualyActive", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "ForceDisable", c => c.Boolean(nullable: false, storeType: "bit"));
        }
    }
}
