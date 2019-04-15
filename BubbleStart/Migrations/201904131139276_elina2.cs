namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class elina2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "StartDay", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "StartDay");
        }
    }
}
