using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class elina22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "ProgramType", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Programs", "ProgramType");
        }
    }
}