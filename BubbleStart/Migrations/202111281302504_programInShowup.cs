namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programInShowup : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.ShowUps", "Program_Id", c => c.Int());
            //CreateIndex("dbo.ShowUps", "Program_Id");
            //AddForeignKey("dbo.ShowUps", "Program_Id", "dbo.Programs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShowUps", "Program_Id", "dbo.Programs");
            DropIndex("dbo.ShowUps", new[] { "Program_Id" });
            DropColumn("dbo.ShowUps", "Program_Id");
        }
    }
}
