namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesProg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramChanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstanceGuid = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.CanceledAppointments");
        }
        
        public override void Down()
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
            
            DropTable("dbo.ProgramChanges");
        }
    }
}
