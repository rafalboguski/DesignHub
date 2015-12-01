namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Nodes", new[] { "Project_Id" });
            AlterColumn("dbo.Nodes", "Project_Id", c => c.Int());
            CreateIndex("dbo.Nodes", "Project_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Nodes", new[] { "Project_Id" });
            AlterColumn("dbo.Nodes", "Project_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Nodes", "Project_Id");
        }
    }
}
