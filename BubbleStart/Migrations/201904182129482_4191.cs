using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _4191 : DbMigration
    {
        public override void Up()
        {
        }

        public override void Down()
        {
            DropTable("dbo.Districts");
        }
    }
}