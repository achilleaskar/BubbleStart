namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _4198 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShowUps", "Amount");
        }

        public override void Down()
        {
            AddColumn("dbo.ShowUps", "Amount", c => c.Int(nullable: false));
        }
    }
}