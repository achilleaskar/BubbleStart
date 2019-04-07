namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class d3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Illnesses", "Eggymosini", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Illnesses", "Eggymosini");
        }
    }
}
