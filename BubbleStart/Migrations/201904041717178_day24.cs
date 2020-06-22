using System.Data.Entity.Migrations;

namespace BubbleStart.Migrations
{
    public partial class day24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Illnesses", "omosd", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "omosA", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "IsxioD", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "isxioA", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "agonasD", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "agonasA", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "gonatoD", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "GonatoA", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "karposD", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "karsposA", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "podoknhmhD", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "PodoknhmhA", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "afxenas", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "thorakas", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Illnesses", "mesh", c => c.String(maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Illnesses", "mesh");
            DropColumn("dbo.Illnesses", "thorakas");
            DropColumn("dbo.Illnesses", "afxenas");
            DropColumn("dbo.Illnesses", "PodoknhmhA");
            DropColumn("dbo.Illnesses", "podoknhmhD");
            DropColumn("dbo.Illnesses", "karsposA");
            DropColumn("dbo.Illnesses", "karposD");
            DropColumn("dbo.Illnesses", "GonatoA");
            DropColumn("dbo.Illnesses", "gonatoD");
            DropColumn("dbo.Illnesses", "agonasA");
            DropColumn("dbo.Illnesses", "agonasD");
            DropColumn("dbo.Illnesses", "isxioA");
            DropColumn("dbo.Illnesses", "IsxioD");
            DropColumn("dbo.Illnesses", "omosA");
            DropColumn("dbo.Illnesses", "omosd");
        }
    }
}