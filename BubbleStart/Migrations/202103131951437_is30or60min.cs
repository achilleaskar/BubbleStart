namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class is30or60min : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShowUps", "Is30min", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "Is30min");
        }
    }
}
