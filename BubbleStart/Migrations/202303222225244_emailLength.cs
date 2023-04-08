namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BubbleCustomers", "Email", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BubbleCustomers", "Email", c => c.String(maxLength: 30, unicode: false));
        }
    }
}
