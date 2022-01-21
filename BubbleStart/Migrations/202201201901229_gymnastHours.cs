namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gymnastHours : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GymnastHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Datetime = c.DateTime(nullable: false, precision: 0),
                        Room = c.Int(nullable: false),
                        Gymnast_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleUsers", t => t.Gymnast_Id)
                .Index(t => t.Gymnast_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GymnastHours", "Gymnast_Id", "dbo.BubbleUsers");
            DropIndex("dbo.GymnastHours", new[] { "Gymnast_Id" });
            DropTable("dbo.GymnastHours");
        }
    }
}
