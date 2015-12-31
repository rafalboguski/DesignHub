namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nodes", "whoAccepted_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Nodes", "whoRejected_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Nodes", "whoAccepted_Id");
            CreateIndex("dbo.Nodes", "whoRejected_Id");
            AddForeignKey("dbo.Nodes", "whoAccepted_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Nodes", "whoRejected_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Nodes", "whoRejected_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Nodes", "whoAccepted_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Nodes", new[] { "whoRejected_Id" });
            DropIndex("dbo.Nodes", new[] { "whoAccepted_Id" });
            DropColumn("dbo.Nodes", "whoRejected_Id");
            DropColumn("dbo.Nodes", "whoAccepted_Id");
        }
    }
}
