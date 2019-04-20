namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

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