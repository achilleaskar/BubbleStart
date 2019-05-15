namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teleftaio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false, precision: 0),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleCustomers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Apointments", "Customer_Id", "dbo.BubbleCustomers");
            DropIndex("dbo.Apointments", new[] { "Customer_Id" });
            DropTable("dbo.Apointments");
        }
    }
}
