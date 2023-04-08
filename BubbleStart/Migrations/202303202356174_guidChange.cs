namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class guidChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgramChanges", "From", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.ProgramChanges", "To", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProgramChanges", "To");
            DropColumn("dbo.ProgramChanges", "From");
        }
    }
}
