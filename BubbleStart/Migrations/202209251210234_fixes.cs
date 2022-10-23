namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InProgramTypes", "ProgramName", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InProgramTypes", "ProgramName", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
