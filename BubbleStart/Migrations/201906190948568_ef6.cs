using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class ef6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BubbleCustomers", "Alcohol", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "Gender", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "HistoryNotFirstTime", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "IsManualyActive", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "Medicine", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "PreferedHand", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "Pregnancy", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "ReasonInjury", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "ReasonPower", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "ReasonSlim", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "ReasonVeltiwsh", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "Smoker", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "Surgery", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.BubbleCustomers", "WantToQuit", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "allergia", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Arthritida", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Asthma", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Diavitis", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Eggymosini", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Emminopafsi", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Epilipsia", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Kardia", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Karkinos", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Katagma", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "NevrikiVlavi", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "omoiopathitiki", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Osteoporosi", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Piesi", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "PiesiXamili", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Stomaxika", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "Thiroidis", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Illnesses", "travmatismos", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Programs", "Paid", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.ShowUps", "Arrive", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShowUps", "Arrive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Programs", "Paid", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "travmatismos", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Thiroidis", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Stomaxika", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "PiesiXamili", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Piesi", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Osteoporosi", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "omoiopathitiki", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "NevrikiVlavi", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Katagma", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Karkinos", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Kardia", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Epilipsia", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Emminopafsi", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Eggymosini", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Diavitis", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Asthma", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "Arthritida", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Illnesses", "allergia", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "WantToQuit", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "Surgery", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "Smoker", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "ReasonVeltiwsh", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "ReasonSlim", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "ReasonPower", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "ReasonInjury", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "Pregnancy", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "PreferedHand", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "Medicine", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "IsManualyActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "HistoryNotFirstTime", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "Gender", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BubbleCustomers", "Alcohol", c => c.Boolean(nullable: false));
        }
    }
}
