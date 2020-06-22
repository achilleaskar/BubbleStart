using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class tel2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Illnesses", "DaxtylaA", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "DaxtylaD", c => c.String(maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Illnesses", "DaxtylaD");
            DropColumn("dbo.Illnesses", "DaxtylaA");
        }
    }
}