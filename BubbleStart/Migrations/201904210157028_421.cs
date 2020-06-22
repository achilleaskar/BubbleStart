using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _421 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Changes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 200, unicode: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Customer_Id = c.Int(),
                        User_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleCustomers", t => t.Customer_Id)
                .ForeignKey("dbo.BubbleUsers", t => t.User_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Changes", "User_Id", "dbo.BubbleUsers");
            DropForeignKey("dbo.Changes", "Customer_Id", "dbo.BubbleCustomers");
            DropIndex("dbo.Changes", new[] { "User_Id" });
            DropIndex("dbo.Changes", new[] { "Customer_Id" });
            DropTable("dbo.Changes");
        }
    }
}
