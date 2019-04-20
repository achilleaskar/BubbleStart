namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class elina : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Programs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DayOfIssue = c.DateTime(nullable: false, precision: 0),
                    Duration = c.Int(nullable: false),
                    Amount = c.Int(nullable: false),
                    Customer_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleCustomers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Programs", "Customer_Id", "dbo.BubbleCustomers");
            DropIndex("dbo.Programs", new[] { "Customer_Id" });
            DropTable("dbo.Programs");
        }
    }
}