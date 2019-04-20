namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class day22 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BubbleCustomers", "HistoryDuration", c => c.Int(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "HistoryKind", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.BubbleCustomers", "HistoryTimesPerWeek", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.BubbleCustomers", "HistoryTimesPerWeek", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "HistoryKind", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "HistoryDuration", c => c.Boolean(nullable: false));
        }
    }
}