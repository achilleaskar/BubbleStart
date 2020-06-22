using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class tel22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Illnesses", "allergia", c => c.Boolean(nullable: false));
            AddColumn("dbo.Illnesses", "allergiaText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "arthritidaText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "asthmaText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "diavitisText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "eggymosynhText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "emmhnopafshText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "epilipsiaText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "kardiaText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "cancerText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "KatagmaText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "nevrikoText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "omoiopathitiki", c => c.Boolean(nullable: false));
            AddColumn("dbo.Illnesses", "opoiopathitikiText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "osteoporosiText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "pieshText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "PiesiXamili", c => c.Boolean(nullable: false));
            AddColumn("dbo.Illnesses", "PiesiXamiliText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "stomaxikaText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "thiroidisText", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "travmatismosText", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Illnesses", "travmatismos", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Illnesses", "travmatismos", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.Illnesses", "travmatismosText");
            DropColumn("dbo.Illnesses", "thiroidisText");
            DropColumn("dbo.Illnesses", "stomaxikaText");
            DropColumn("dbo.Illnesses", "PiesiXamiliText");
            DropColumn("dbo.Illnesses", "PiesiXamili");
            DropColumn("dbo.Illnesses", "pieshText");
            DropColumn("dbo.Illnesses", "osteoporosiText");
            DropColumn("dbo.Illnesses", "opoiopathitikiText");
            DropColumn("dbo.Illnesses", "omoiopathitiki");
            DropColumn("dbo.Illnesses", "nevrikoText");
            DropColumn("dbo.Illnesses", "KatagmaText");
            DropColumn("dbo.Illnesses", "cancerText");
            DropColumn("dbo.Illnesses", "kardiaText");
            DropColumn("dbo.Illnesses", "epilipsiaText");
            DropColumn("dbo.Illnesses", "emmhnopafshText");
            DropColumn("dbo.Illnesses", "eggymosynhText");
            DropColumn("dbo.Illnesses", "diavitisText");
            DropColumn("dbo.Illnesses", "asthmaText");
            DropColumn("dbo.Illnesses", "arthritidaText");
            DropColumn("dbo.Illnesses", "allergiaText");
            DropColumn("dbo.Illnesses", "allergia");
        }
    }
}