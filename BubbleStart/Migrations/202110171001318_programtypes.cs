namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programtypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InProgramTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramName = c.String(maxLength: 200, unicode: false),
                        ProgramMode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Programs", "ProgramTypeO_Id", c => c.Int());
            CreateIndex("dbo.Programs", "ProgramTypeO_Id");
            AddForeignKey("dbo.Programs", "ProgramTypeO_Id", "dbo.InProgramTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Programs", "ProgramTypeO_Id", "dbo.InProgramTypes");
            DropIndex("dbo.Programs", new[] { "ProgramTypeO_Id" });
            DropColumn("dbo.Programs", "ProgramTypeO_Id");
            DropTable("dbo.InProgramTypes");
        }
    }
}
