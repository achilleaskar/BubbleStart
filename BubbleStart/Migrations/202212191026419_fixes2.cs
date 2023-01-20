namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixes2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "IllDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.ShowUps", "Gymnast_Id", c => c.Int());
            CreateIndex("dbo.ShowUps", "Gymnast_Id");
            AddForeignKey("dbo.ShowUps", "Gymnast_Id", "dbo.BubbleUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShowUps", "Gymnast_Id", "dbo.BubbleUsers");
            DropIndex("dbo.ShowUps", new[] { "Gymnast_Id" });
            DropColumn("dbo.ShowUps", "Gymnast_Id");
            DropColumn("dbo.BubbleCustomers", "IllDate");
        }
    }
}
