namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bubble : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("ShowUps", "Prog_Id", "Programs");
            AddForeignKey("ShowUps", "Prog_Id", "Programs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ShowUps", "Prog_Id", "Programs");
            AddForeignKey("ShowUps", "Prog_Id", "Programs", "Id", cascadeDelete: true);
        }
    }
}
