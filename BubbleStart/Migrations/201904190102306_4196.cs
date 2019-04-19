namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4196 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "ExtraReasons", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "ExtraReasons");
        }
    }
}
