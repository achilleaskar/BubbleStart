namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecieptNMethod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BubblePayments", "Reciept", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.BubblePayments", "PaymentType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BubblePayments", "PaymentType");
            DropColumn("dbo.BubblePayments", "Reciept");
        }
    }
}
