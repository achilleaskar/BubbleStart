using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _4195 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "ExtraNotes", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.BubbleCustomers", "HistoryDuration", c => c.String(maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.BubbleCustomers", "HistoryDuration", c => c.Int(nullable: false));
            DropColumn("dbo.BubbleCustomers", "ExtraNotes");
        }
    }
}