namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class progs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShowUps", "Prog_Id", c => c.Int());
            CreateIndex("dbo.ShowUps", "Prog_Id");
            AddForeignKey("dbo.ShowUps", "Prog_Id", "dbo.Programs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShowUps", "Prog_Id", "dbo.Programs");
            DropIndex("dbo.ShowUps", new[] { "Prog_Id" });
            DropColumn("dbo.ShowUps", "Prog_Id");
        }
    }
}
