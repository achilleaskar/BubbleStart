namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_reciept : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Expenses", "HasReciept");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "HasReciept", c => c.Boolean(nullable: false, storeType: "bit"));
        }
    }
}
