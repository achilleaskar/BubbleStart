namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vacinated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "Vacinated", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "Vacinated");
        }
    }
}
