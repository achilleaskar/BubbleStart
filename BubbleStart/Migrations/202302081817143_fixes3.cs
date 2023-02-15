namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixes3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apointments", "Waiting", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.Apointments", "Canceled", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.BubbleCustomers", "DefaultProgramModes", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "DefaultProgramModes");
            DropColumn("dbo.Apointments", "Canceled");
            DropColumn("dbo.Apointments", "Waiting");
        }
    }
}
