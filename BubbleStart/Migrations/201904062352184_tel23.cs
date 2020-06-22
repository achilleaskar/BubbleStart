using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class tel23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "MedicineText", c => c.String(maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "MedicineText");
        }
    }
}