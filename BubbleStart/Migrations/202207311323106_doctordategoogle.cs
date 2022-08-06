namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctordategoogle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "DoctorDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.BubbleCustomers", "Google", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "Google");
            DropColumn("dbo.BubbleCustomers", "DoctorDate");
        }
    }
}
