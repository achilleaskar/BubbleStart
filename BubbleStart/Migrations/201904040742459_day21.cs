namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class day21 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShowUps", "Paid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShowUps", "Paid", c => c.Boolean(nullable: false));
        }
    }
}
