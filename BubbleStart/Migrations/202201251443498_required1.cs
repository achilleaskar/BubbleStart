namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class required1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShowUps", "Test", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "Test");
        }
    }
}
