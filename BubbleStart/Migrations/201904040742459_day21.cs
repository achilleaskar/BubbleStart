using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class day21 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShowUps", "Paid");
        }

        public override void Down()
        {
            AddColumn("dbo.ShowUps", "Paid", c => c.Boolean(nullable: false));
        }
    }
}