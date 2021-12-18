namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Programs", "DealId", c => c.Int());
            CreateIndex("dbo.Programs", "DealId");
            AddForeignKey("dbo.Programs", "DealId", "dbo.Deals", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Programs", "DealId", "dbo.Deals");
            DropIndex("dbo.Programs", new[] { "DealId" });
            DropColumn("dbo.Programs", "DealId");
            DropTable("dbo.Deals");
        }
    }
}
