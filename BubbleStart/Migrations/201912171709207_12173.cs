using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _12173 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubblePayments", "Program_Id", c => c.Int());
            AddColumn("dbo.Programs", "RemainingAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.BubblePayments", "Program_Id");
            AddForeignKey("dbo.BubblePayments", "Program_Id", "dbo.Programs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BubblePayments", "Program_Id", "dbo.Programs");
            DropIndex("dbo.BubblePayments", new[] { "Program_Id" });
            DropColumn("dbo.Programs", "RemainingAmount");
            DropColumn("dbo.BubblePayments", "Program_Id");
        }
    }
}
