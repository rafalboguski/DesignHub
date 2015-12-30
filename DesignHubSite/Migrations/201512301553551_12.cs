namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nodes", "Accepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Nodes", "Rejected", c => c.Boolean(nullable: false));
            AddColumn("dbo.Nodes", "Likes", c => c.Int(nullable: false));
            AddColumn("dbo.Nodes", "Dislikes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Nodes", "Dislikes");
            DropColumn("dbo.Nodes", "Likes");
            DropColumn("dbo.Nodes", "Rejected");
            DropColumn("dbo.Nodes", "Accepted");
        }
    }
}
