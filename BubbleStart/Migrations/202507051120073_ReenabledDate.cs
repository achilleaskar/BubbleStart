namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ReenabledDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "ReEnabled", c => c.DateTime(nullable: false, precision: 0));
        }

        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "ReEnabled");
        }
    }
}