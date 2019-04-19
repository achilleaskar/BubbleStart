namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _418 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "DistrictText", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "DistrictText");
        }
    }
}
