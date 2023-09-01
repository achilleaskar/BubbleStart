namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreveee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GymnastHours", "Forever", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GymnastHours", "Forever");
        }
    }
}
