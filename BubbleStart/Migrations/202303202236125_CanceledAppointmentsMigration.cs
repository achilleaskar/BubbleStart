namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CanceledAppointmentsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CanceledAppointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        DeletedDate = c.DateTime(nullable: false, precision: 0),
                        AppointmentDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CanceledAppointments");
        }
    }
}
