namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kyriakh : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apointments", "Room");
            DropColumn("dbo.Apointments", "Person");
        }
    }
}
