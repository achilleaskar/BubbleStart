namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class person_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Apointments", "Person", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Apointments", "Person", c => c.Int(nullable: false));
        }
    }
}
