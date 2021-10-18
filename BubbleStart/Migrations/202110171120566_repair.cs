namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repair : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "ProgramType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "ProgramType");
        }
    }
}
