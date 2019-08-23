namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8122 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BubbleCustomers", "Illness_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.BubbleCustomers", "Illness_Id");
            AddForeignKey("dbo.BubbleCustomers", "Illness_Id", "dbo.Illnesses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BubbleCustomers", "Illness_Id", "dbo.Illnesses");
            DropIndex("dbo.BubbleCustomers", new[] { "Illness_Id" });
            AlterColumn("dbo.BubbleCustomers", "Illness_Id", c => c.Int());
            CreateIndex("dbo.BubbleCustomers", "Illness_Id");
            AddForeignKey("dbo.BubbleCustomers", "Illness_Id", "dbo.Illnesses", "Id");
        }
    }
}
