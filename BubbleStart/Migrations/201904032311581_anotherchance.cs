namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherchance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShowUps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false, precision: 0),
                        Arrive = c.Boolean(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleCustomers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShowUps", "Customer_Id", "dbo.BubbleCustomers");
            DropIndex("dbo.ShowUps", new[] { "Customer_Id" });
            DropTable("dbo.ShowUps");
        }
    }
}
