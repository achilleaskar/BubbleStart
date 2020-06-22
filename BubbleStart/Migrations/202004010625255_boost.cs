namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boost : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BubbleCustomers", "Pregnancy");
            DropColumn("dbo.BubbleCustomers", "Signed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BubbleCustomers", "Signed", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.BubbleCustomers", "Pregnancy", c => c.Boolean(nullable: false, storeType: "bit"));
        }
    }
}
