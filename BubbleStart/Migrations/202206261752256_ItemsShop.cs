namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemsShop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Items", "ColorString", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Items", "Shop", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Shop");
            DropColumn("dbo.Items", "ColorString");
            DropColumn("dbo.Items", "Price");
        }
    }
}
