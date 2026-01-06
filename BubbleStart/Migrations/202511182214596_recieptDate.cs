namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recieptDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "RecieptDate", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "RecieptDate");
        }
    }
}
