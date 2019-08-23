namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1115 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "ForceDisable", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "ForceDisable");
        }
    }
}
