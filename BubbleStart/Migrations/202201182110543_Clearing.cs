namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Clearing : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Programs", "StrictDuration");
            DropColumn("dbo.Expenses", "ExpenseCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "ExpenseCategory", c => c.Int(nullable: false));
            AddColumn("dbo.Programs", "StrictDuration", c => c.Boolean(nullable: false, storeType: "bit"));
        }
    }
}
