using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _1115 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "ForceDisable", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "ForceDisable");
        }
    }
}
