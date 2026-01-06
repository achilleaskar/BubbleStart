namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resSemDay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "ResetSemDay", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "ResetSemDay");
        }
    }
}
