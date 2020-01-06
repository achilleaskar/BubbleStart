namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1220 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShowUps", "Massage", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "Massage");
        }
    }
}
