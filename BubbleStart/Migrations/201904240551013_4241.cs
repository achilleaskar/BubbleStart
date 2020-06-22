using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _4241 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Reason = c.String(maxLength: 200, unicode: false),
                        Amount = c.Single(nullable: false),
                        User_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "User_Id", "dbo.BubbleUsers");
            DropIndex("dbo.Expenses", new[] { "User_Id" });
            DropTable("dbo.Expenses");
        }
    }
}
