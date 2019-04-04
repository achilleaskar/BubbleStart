namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherchance2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShowUps", "Amount", c => c.Int(nullable: false));
            AddColumn("dbo.ShowUps", "Paid", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShowUps", "Left", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.ShowUps", "Arrived", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.ShowUps", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShowUps", "Time", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.ShowUps", "Arrived");
            DropColumn("dbo.ShowUps", "Left");
            DropColumn("dbo.ShowUps", "Paid");
            DropColumn("dbo.ShowUps", "Amount");
        }
    }
}
