namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "Gift", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "Gift");
        }
    }
}
