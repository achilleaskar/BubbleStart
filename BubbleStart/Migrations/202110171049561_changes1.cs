namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Programs", "ProgramType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Programs", "ProgramType", c => c.Int(nullable: false));
        }
    }
}
