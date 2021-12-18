namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stringDuration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "StrictDuration", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "StrictDuration");
        }
    }
}
