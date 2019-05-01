namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _422 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BubblePayments", "Amount", c => c.Single(nullable: false));
            AlterColumn("dbo.Programs", "Amount", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Programs", "Amount", c => c.Int(nullable: false));
            AlterColumn("dbo.BubblePayments", "Amount", c => c.Int(nullable: false));
        }
    }
}
