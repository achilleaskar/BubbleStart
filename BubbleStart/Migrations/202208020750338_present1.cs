namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class present1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemPurchases", "Free", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemPurchases", "Free");
        }
    }
}
