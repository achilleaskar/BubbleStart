namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shifts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BubbleWorkingRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false, precision: 0),
                        To = c.DateTime(nullable: false, precision: 0),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.BubbleDayWorkingShifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumOfDay = c.Int(nullable: false),
                        Shift_Id = c.Int(),
                        WorkingRule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleShifts", t => t.Shift_Id)
                .ForeignKey("dbo.BubbleWorkingRules", t => t.WorkingRule_Id)
                .Index(t => t.Shift_Id)
                .Index(t => t.WorkingRule_Id);
            
            CreateTable(
                "dbo.BubbleShifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false, precision: 0),
                        FromB = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 200, unicode: false),
                        Parted = c.Boolean(nullable: false, storeType: "bit"),
                        To = c.DateTime(nullable: false, precision: 0),
                        ToB = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BubbleWorkingRules", "User_Id", "dbo.BubbleUsers");
            DropForeignKey("dbo.BubbleDayWorkingShifts", "WorkingRule_Id", "dbo.BubbleWorkingRules");
            DropForeignKey("dbo.BubbleDayWorkingShifts", "Shift_Id", "dbo.BubbleShifts");
            DropIndex("dbo.BubbleDayWorkingShifts", new[] { "WorkingRule_Id" });
            DropIndex("dbo.BubbleDayWorkingShifts", new[] { "Shift_Id" });
            DropIndex("dbo.BubbleWorkingRules", new[] { "User_Id" });
            DropTable("dbo.BubbleShifts");
            DropTable("dbo.BubbleDayWorkingShifts");
            DropTable("dbo.BubbleWorkingRules");
        }
    }
}
