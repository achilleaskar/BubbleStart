using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class kyriakh : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apointments", "Room");
            DropColumn("dbo.Apointments", "Person");
        }
    }
}
