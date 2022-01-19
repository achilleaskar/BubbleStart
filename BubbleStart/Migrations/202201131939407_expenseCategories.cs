namespace BubbleStart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expenseCategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpenseCategoryClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50, unicode: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExpenseCategoryClasses", t => t.ParentId)
                .Index(t => t.ParentId);
            
            AddColumn("dbo.Expenses", "MainCategoryId", c => c.Int());
            AddColumn("dbo.Expenses", "SecondaryCategoryId", c => c.Int());
            CreateIndex("dbo.Expenses", "MainCategoryId");
            CreateIndex("dbo.Expenses", "SecondaryCategoryId");
            AddForeignKey("dbo.Expenses", "MainCategoryId", "dbo.ExpenseCategoryClasses", "Id");
            AddForeignKey("dbo.Expenses", "SecondaryCategoryId", "dbo.ExpenseCategoryClasses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "SecondaryCategoryId", "dbo.ExpenseCategoryClasses");
            DropForeignKey("dbo.Expenses", "MainCategoryId", "dbo.ExpenseCategoryClasses");
            DropForeignKey("dbo.ExpenseCategoryClasses", "ParentId", "dbo.ExpenseCategoryClasses");
            DropIndex("dbo.ExpenseCategoryClasses", new[] { "ParentId" });
            DropIndex("dbo.Expenses", new[] { "SecondaryCategoryId" });
            DropIndex("dbo.Expenses", new[] { "MainCategoryId" });
            DropColumn("dbo.Expenses", "SecondaryCategoryId");
            DropColumn("dbo.Expenses", "MainCategoryId");
            DropTable("dbo.ExpenseCategoryClasses");
        }
    }
}
