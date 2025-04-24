namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reciepts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Reciept", c => c.Boolean());
            AddColumn("dbo.Expenses", "SelectedStore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "SelectedStore");
            DropColumn("dbo.Expenses", "Reciept");
        }
    }
}
