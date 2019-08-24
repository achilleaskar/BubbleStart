namespace BubbleStart.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class _1234 : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.ShowUps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Arrive = c.Boolean(nullable: false, storeType: "bit"),
                        Arrived = c.DateTime(nullable: false, precision: 0),
                        Left = c.DateTime(nullable: false, precision: 0),
                        Customer_Id = c.Int(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ShowUp_Arrived",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
        }
        
        public override void Down()
        {
            AlterTableAnnotations(
                "dbo.ShowUps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Arrive = c.Boolean(nullable: false, storeType: "bit"),
                        Arrived = c.DateTime(nullable: false, precision: 0),
                        Left = c.DateTime(nullable: false, precision: 0),
                        Customer_Id = c.Int(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ShowUp_Arrived",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
    }
}
