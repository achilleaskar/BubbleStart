namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gymanst : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apointments", "GymnastId", c => c.Int());
            CreateIndex("dbo.Apointments", "GymnastId");
            AddForeignKey("dbo.Apointments", "GymnastId", "dbo.BubbleUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Apointments", "GymnastId", "dbo.BubbleUsers");
            DropIndex("dbo.Apointments", new[] { "GymnastId" });
            DropColumn("dbo.Apointments", "GymnastId");
        }
    }
}
