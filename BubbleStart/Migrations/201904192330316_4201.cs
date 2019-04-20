namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4201 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "Months", c => c.Int(nullable: false));
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.Programs", "Duration", c => c.Int(nullable: false));
            DropColumn("dbo.Programs", "Showups");
            DropColumn("dbo.Programs", "Months");
        }
    }
}
