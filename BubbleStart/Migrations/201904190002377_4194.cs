namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _4194 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BubbleCustomers", "MyProperty");
        }

        public override void Down()
        {
            AddColumn("dbo.BubbleCustomers", "MyProperty", c => c.Boolean(nullable: false));
        }
    }
}