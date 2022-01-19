namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incomes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Income", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "Income");
        }
    }
}
