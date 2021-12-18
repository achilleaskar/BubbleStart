namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BodyPart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShowUps", "BodyPart", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "BodyPart");
        }
    }
}
