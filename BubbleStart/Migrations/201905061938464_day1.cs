namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class day1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BubblePayments", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Programs", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Weights", "WeightValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Expenses", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Expenses", "Amount", c => c.Single(nullable: false));
            AlterColumn("dbo.Weights", "WeightValue", c => c.Single(nullable: false));
            AlterColumn("dbo.Programs", "Amount", c => c.Single(nullable: false));
            AlterColumn("dbo.BubblePayments", "Amount", c => c.Single(nullable: false));
        }
    }
}
