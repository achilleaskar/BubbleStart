namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascade : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("Changes", "Payment_Id", "dbo.BubblePayments");
            //DropForeignKey("Changes", "Program_Id", "dbo.Programs");
            //DropForeignKey("Changes", "ShowUp_Id", "dbo.ShowUps");
            AddForeignKey("Changes", "Payment_Id", "dbo.BubblePayments", "Id", cascadeDelete: true);
            AddForeignKey("Changes", "Program_Id", "dbo.Programs", "Id", cascadeDelete: true);
            AddForeignKey("Changes", "ShowUp_Id", "dbo.ShowUps", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Changes", "ShowUp_Id", "dbo.ShowUps");
            DropForeignKey("dbo.Changes", "Program_Id", "dbo.Programs");
            DropForeignKey("dbo.Changes", "Payment_Id", "dbo.BubblePayments");
            AddForeignKey("dbo.Changes", "ShowUp_Id", "dbo.ShowUps", "Id");
            AddForeignKey("dbo.Changes", "Program_Id", "dbo.Programs", "Id");
            AddForeignKey("dbo.Changes", "Payment_Id", "dbo.BubblePayments", "Id");
        }
    }
}
