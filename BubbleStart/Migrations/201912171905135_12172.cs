namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12172 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Programs", "RemainingAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Programs", "RemainingAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
