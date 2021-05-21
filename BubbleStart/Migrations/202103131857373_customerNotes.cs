namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customerNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "Notes", c => c.String(maxLength: 500, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "Notes");
        }
    }
}
