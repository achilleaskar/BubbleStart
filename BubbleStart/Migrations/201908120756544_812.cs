using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class _812 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "Signed", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "Signed");
        }
    }
}
