namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class outdoor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "ProgramMode", c => c.Int(nullable: false));
            AddColumn("dbo.ShowUps", "ProgramMode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "ProgramMode");
            DropColumn("dbo.Programs", "ProgramMode");
        }
    }
}
