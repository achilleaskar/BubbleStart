namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class colors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemPurchases", "ClothColors", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemPurchases", "ClothColors");
        }
    }
}
