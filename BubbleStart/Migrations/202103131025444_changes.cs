namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Changes", "Payment_Id", c => c.Int());
            AddColumn("dbo.Changes", "Program_Id", c => c.Int());
            AddColumn("dbo.Changes", "ShowUp_Id", c => c.Int());
            CreateIndex("dbo.Changes", "Payment_Id");
            CreateIndex("dbo.Changes", "Program_Id");
            CreateIndex("dbo.Changes", "ShowUp_Id");
            AddForeignKey("dbo.Changes", "Payment_Id", "dbo.BubblePayments", "Id");
            AddForeignKey("dbo.Changes", "Program_Id", "dbo.Programs", "Id");
            AddForeignKey("dbo.Changes", "ShowUp_Id", "dbo.ShowUps", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Changes", "ShowUp_Id", "dbo.ShowUps");
            DropForeignKey("dbo.Changes", "Program_Id", "dbo.Programs");
            DropForeignKey("dbo.Changes", "Payment_Id", "dbo.BubblePayments");
            DropIndex("dbo.Changes", new[] { "ShowUp_Id" });
            DropIndex("dbo.Changes", new[] { "Program_Id" });
            DropIndex("dbo.Changes", new[] { "Payment_Id" });
            DropColumn("dbo.Changes", "ShowUp_Id");
            DropColumn("dbo.Changes", "Program_Id");
            DropColumn("dbo.Changes", "Payment_Id");
        }
    }
}
