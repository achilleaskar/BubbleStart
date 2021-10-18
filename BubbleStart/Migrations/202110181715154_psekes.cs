namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class psekes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "Psek", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "Psek");
        }
    }
}
