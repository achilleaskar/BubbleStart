namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemsShop2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemPurchases", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ItemPurchases", "ColorString", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.Items", "Price");
            DropColumn("dbo.Items", "ColorString");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "ColorString", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Items", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ItemPurchases", "ColorString");
            DropColumn("dbo.ItemPurchases", "Price");
        }
    }
}
