namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class day23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "Surgery", c => c.Boolean(nullable: false));
            AddColumn("dbo.BubbleCustomers", "SurgeryInfo", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.BubbleCustomers", "Medicine", c => c.Boolean(nullable: false));
            AddColumn("dbo.BubbleCustomers", "Pregnancy", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "Pregnancy");
            DropColumn("dbo.BubbleCustomers", "Medicine");
            DropColumn("dbo.BubbleCustomers", "SurgeryInfo");
            DropColumn("dbo.BubbleCustomers", "Surgery");
        }
    }
}