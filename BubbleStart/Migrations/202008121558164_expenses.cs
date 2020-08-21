namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expenses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "ExpenseCategory", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "ExpenseCategory");
        }
    }
}
