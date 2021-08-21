namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class items : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemPurchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Size = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        ItemId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleCustomers", t => t.CustomerId)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.CustomerId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 40, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemPurchases", "ItemId", "dbo.Items");
            DropForeignKey("dbo.ItemPurchases", "CustomerId", "dbo.BubbleCustomers");
            DropIndex("dbo.ItemPurchases", new[] { "ItemId" });
            DropIndex("dbo.ItemPurchases", new[] { "CustomerId" });
            DropTable("dbo.Items");
            DropTable("dbo.ItemPurchases");
        }
    }
}
