namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class corrections : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExpenseCategoryClasses", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExpenseCategoryClasses", "Name", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
