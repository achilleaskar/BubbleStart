using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _4201 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "Months", c => c.Int(nullable: false));
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.Programs", "Duration", c => c.Int(nullable: false));
            DropColumn("dbo.Programs", "Showups");
            DropColumn("dbo.Programs", "Months");
        }
    }
}
