namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Node_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Node_Id1", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Node_Id");
            CreateIndex("dbo.AspNetUsers", "Node_Id1");
            AddForeignKey("dbo.AspNetUsers", "Node_Id", "dbo.Nodes", "Id");
            AddForeignKey("dbo.AspNetUsers", "Node_Id1", "dbo.Nodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Node_Id1", "dbo.Nodes");
            DropForeignKey("dbo.AspNetUsers", "Node_Id", "dbo.Nodes");
            DropIndex("dbo.AspNetUsers", new[] { "Node_Id1" });
            DropIndex("dbo.AspNetUsers", new[] { "Node_Id" });
            DropColumn("dbo.AspNetUsers", "Node_Id1");
            DropColumn("dbo.AspNetUsers", "Node_Id");
        }
    }
}
