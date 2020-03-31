namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _323 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "Enabled", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.ShowUps", "Real", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "Real");
            DropColumn("dbo.BubbleCustomers", "Enabled");
        }
    }
}
