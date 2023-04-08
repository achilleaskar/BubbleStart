namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifiedappo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apointments", "Modified", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apointments", "Modified");
        }
    }
}
