namespace BubbleStart.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class lastmin1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BubbleCustomers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Address = c.String(maxLength: 200, unicode: false),
                    Alcohol = c.Boolean(nullable: false),
                    AlcoholUsage = c.Int(nullable: false),
                    District = c.Int(nullable: false),
                    DOB = c.DateTime(nullable: false, precision: 0),
                    Email = c.String(maxLength: 30, unicode: false),
                    FirstDate = c.DateTime(nullable: false, precision: 0),
                    Gender = c.Boolean(nullable: false),
                    Height = c.Int(nullable: false),
                    HistoryDuration = c.Boolean(nullable: false),
                    HistoryKind = c.Boolean(nullable: false),
                    HistoryNotFirstTime = c.Boolean(nullable: false),
                    HistoryTimesPerWeek = c.Boolean(nullable: false),
                    Job = c.String(maxLength: 200, unicode: false),
                    MyProperty = c.Boolean(nullable: false),
                    Name = c.String(nullable: false, maxLength: 20, unicode: false),
                    PreferedHand = c.Boolean(nullable: false),
                    ReasonInjury = c.Boolean(nullable: false),
                    ReasonPower = c.Boolean(nullable: false),
                    ReasonSlim = c.Boolean(nullable: false),
                    Smoker = c.Boolean(nullable: false),
                    SmokingUsage = c.Int(nullable: false),
                    SureName = c.String(nullable: false, maxLength: 20, unicode: false),
                    Tel = c.String(nullable: false, maxLength: 18, unicode: false),
                    WantToQuit = c.Boolean(nullable: false),
                    Illness_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Illnesses", t => t.Illness_Id)
                .Index(t => t.Illness_Id);

            CreateTable(
                "dbo.Illnesses",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Arthritida = c.Boolean(nullable: false),
                    Asthma = c.Boolean(nullable: false),
                    Diavitis = c.Boolean(nullable: false),
                    Emminopafsi = c.Boolean(nullable: false),
                    Epilipsia = c.Boolean(nullable: false),
                    Kardia = c.Boolean(nullable: false),
                    Karkinos = c.Boolean(nullable: false),
                    Katagma = c.Boolean(nullable: false),
                    NevrikiVlavi = c.Boolean(nullable: false),
                    Osteoporosi = c.Boolean(nullable: false),
                    Piesi = c.Boolean(nullable: false),
                    Stomaxika = c.Boolean(nullable: false),
                    Thiroidis = c.Boolean(nullable: false),
                    Travmatismos = c.String(maxLength: 200, unicode: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Weights",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DateOfMeasure = c.DateTime(nullable: false, precision: 0),
                    WeightValue = c.Single(nullable: false),
                    Customer_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BubbleCustomers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Weights", "Customer_Id", "dbo.BubbleCustomers");
            DropForeignKey("dbo.BubbleCustomers", "Illness_Id", "dbo.Illnesses");
            DropIndex("dbo.Weights", new[] { "Customer_Id" });
            DropIndex("dbo.BubbleCustomers", new[] { "Illness_Id" });
            DropTable("dbo.Weights");
            DropTable("dbo.Illnesses");
            DropTable("dbo.BubbleCustomers");
        }
    }
}