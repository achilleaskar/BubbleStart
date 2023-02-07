namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "MassageResetDay", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.InProgramTypes", "ProgramName", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InProgramTypes", "ProgramName", c => c.String(maxLength: 100, unicode: false));
            DropColumn("dbo.BubbleCustomers", "MassageResetDay");
        }
    }
}
