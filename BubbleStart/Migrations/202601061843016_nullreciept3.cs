namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullreciept3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Expenses", "Reciept", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Expenses", "Reciept", c => c.Boolean());
        }
    }
}
