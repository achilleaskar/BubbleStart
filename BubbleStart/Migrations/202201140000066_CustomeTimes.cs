namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomeTimes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomeTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.String(maxLength: 10, unicode: false),
                        Datetime = c.DateTime(nullable: false, precision: 0),
                        Room = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomeTimes");
        }
    }
}
