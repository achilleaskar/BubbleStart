namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _4181 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Districts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 200, unicode: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.BubbleCustomers", "District_Id", c => c.Int());
            CreateIndex("dbo.BubbleCustomers", "District_Id");
            AddForeignKey("dbo.BubbleCustomers", "District_Id", "dbo.Districts", "Id");
            DropColumn("dbo.BubbleCustomers", "District");
        }

        public override void Down()
        {
            AddColumn("dbo.BubbleCustomers", "District", c => c.Int(nullable: false));
            DropForeignKey("dbo.BubbleCustomers", "District_Id", "dbo.Districts");
            DropIndex("dbo.BubbleCustomers", new[] { "District_Id" });
            DropColumn("dbo.BubbleCustomers", "District_Id");
            DropTable("dbo.Districts");
        }
    }
}