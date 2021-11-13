namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expenseduration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "From", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Expenses", "To", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "To");
            DropColumn("dbo.Expenses", "From");
        }
    }
}
