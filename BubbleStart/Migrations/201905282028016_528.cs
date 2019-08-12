namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _528 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "Paid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "Paid");
        }
    }
}
