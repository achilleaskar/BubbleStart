namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

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