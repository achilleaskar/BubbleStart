namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecBodyPart_Doctor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubbleCustomers", "Doctor", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.ShowUps", "SecBodyPartsString", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShowUps", "SecBodyPartsString");
            DropColumn("dbo.BubbleCustomers", "Doctor");
        }
    }
}
