namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xilia : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BubbleCustomers", "Notes", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BubbleCustomers", "Notes", c => c.String(maxLength: 500, unicode: false));
        }
    }
}
