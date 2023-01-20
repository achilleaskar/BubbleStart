namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsSelectedProgramTypeConverter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "DefaultProgramType_Id", c => c.Int());
            CreateIndex("dbo.BubbleCustomers", "DefaultProgramType_Id");
            AddForeignKey("dbo.BubbleCustomers", "DefaultProgramType_Id", "dbo.InProgramTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BubbleCustomers", "DefaultProgramType_Id", "dbo.InProgramTypes");
            DropIndex("dbo.BubbleCustomers", new[] { "DefaultProgramType_Id" });
            DropColumn("dbo.BubbleCustomers", "DefaultProgramType_Id");
        }
    }
}
