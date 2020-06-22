using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class first2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BubbleUsers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    HashedPassword = c.Binary(),
                    Level = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 20, unicode: false),
                    Surename = c.String(nullable: false, maxLength: 20, unicode: false),
                    Tel = c.String(nullable: false, maxLength: 18, unicode: false),
                    UserName = c.String(nullable: false, maxLength: 12, unicode: false)
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.BubbleUsers");
        }
    }
}