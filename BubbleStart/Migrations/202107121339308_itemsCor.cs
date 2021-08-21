namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itemsCor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ItemPurchases", "Size", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ItemPurchases", "Size", c => c.Int(nullable: false));
        }
    }
}
