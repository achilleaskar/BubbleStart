namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class present : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShowUps", "Present", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "Present");
        }
    }
}
