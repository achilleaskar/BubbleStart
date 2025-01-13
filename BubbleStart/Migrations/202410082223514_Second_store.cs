namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second_store : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgramChanges", "GymNum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProgramChanges", "GymNum");
        }
    }
}
