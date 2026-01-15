namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reciept : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "HasReciept", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "HasReciept");
        }
    }
}
