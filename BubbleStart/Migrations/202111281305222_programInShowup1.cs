namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programInShowup1 : DbMigration
    {
        public override void Up()
        {
           //DropForeignKey("ShowUps", "Program_Id", "dbo.Programs");
           //DropIndex("ShowUps", new[] { "Program_Id" });
            //DropColumn("ShowUps", "Program_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShowUps", "Program_Id", c => c.Int());
            CreateIndex("dbo.ShowUps", "Program_Id");
            AddForeignKey("dbo.ShowUps", "Program_Id", "dbo.Programs", "Id");
        }
    }
}
