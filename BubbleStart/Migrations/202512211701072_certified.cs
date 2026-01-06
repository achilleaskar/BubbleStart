namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class certified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "Certified", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "Certified");
        }
    }
}
