namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsSelectedProgramTypeConverterfix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("BubbleCustomers", "DefaultProgramType_Id", "dbo.InProgramTypes");
            DropIndex("BubbleCustomers", new[] { "DefaultProgramType_Id" });
            AddColumn("BubbleCustomers", "DefaultProgramMode", c => c.Int(nullable: false));
            DropColumn("BubbleCustomers", "DefaultProgramType_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BubbleCustomers", "DefaultProgramType_Id", c => c.Int());
            DropColumn("dbo.BubbleCustomers", "DefaultProgramMode");
            CreateIndex("dbo.BubbleCustomers", "DefaultProgramType_Id");
            AddForeignKey("dbo.BubbleCustomers", "DefaultProgramType_Id", "dbo.InProgramTypes", "Id");
        }
    }
}
