namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class colorname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemPurchases", "Color", c => c.Int());
            DropColumn("dbo.ItemPurchases", "ClothColors");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemPurchases", "ClothColors", c => c.Int());
            DropColumn("dbo.ItemPurchases", "Color");
        }
    }
}
