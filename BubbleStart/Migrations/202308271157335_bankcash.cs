namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bankcash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Bank", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Expenses", "Cash", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "Cash");
            DropColumn("dbo.Expenses", "Bank");
        }
    }
}
