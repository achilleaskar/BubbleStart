namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class disableUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleUsers", "Disabled", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleUsers", "Disabled");
        }
    }
}
