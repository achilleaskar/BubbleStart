using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class tel21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "ReasonVeltiwsh", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "ReasonVeltiwsh");
        }
    }
}