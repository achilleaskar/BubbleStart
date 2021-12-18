namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class progs1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("ShowUps", "Prog_Id", "Programs");
            AddForeignKey("ShowUps", "Prog_Id", "Programs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShowUps", "Prog_Id", "dbo.Programs");
            AddForeignKey("dbo.ShowUps", "Prog_Id", "dbo.Programs", "Id");
        }
    }
}
