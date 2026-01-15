namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class google : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "PilatesStudio", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.BubbleCustomers", "FunctionalStudio", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.BubbleCustomers", "CorpusAthleticSeminars", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.BubbleCustomers", "Book", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubbleCustomers", "Book");
            DropColumn("dbo.BubbleCustomers", "CorpusAthleticSeminars");
            DropColumn("dbo.BubbleCustomers", "FunctionalStudio");
            DropColumn("dbo.BubbleCustomers", "PilatesStudio");
        }
    }
}
