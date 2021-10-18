namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programmodenew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShowUps", "ProgramModeNew", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "ProgramModeNew");
        }
    }
}
