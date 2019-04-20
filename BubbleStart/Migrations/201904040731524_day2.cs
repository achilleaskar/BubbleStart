namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class day2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BubblePayments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false, precision: 0),
                    Amount = c.Int(nullable: false),
                    Customer_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleCustomers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.BubblePayments", "Customer_Id", "dbo.BubbleCustomers");
            DropIndex("dbo.BubblePayments", new[] { "Customer_Id" });
            DropTable("dbo.BubblePayments");
        }
    }
}