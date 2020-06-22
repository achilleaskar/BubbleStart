using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _424 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubblePayments", "User_Id", c => c.Int());
            CreateIndex("dbo.BubblePayments", "User_Id");
            AddForeignKey("dbo.BubblePayments", "User_Id", "dbo.BubbleUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BubblePayments", "User_Id", "dbo.BubbleUsers");
            DropIndex("dbo.BubblePayments", new[] { "User_Id" });
            DropColumn("dbo.BubblePayments", "User_Id");
        }
    }
}
