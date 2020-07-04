namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class outdoor : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "ProgramMode");
            DropColumn("dbo.Programs", "ProgramMode");
        }
    }
}
