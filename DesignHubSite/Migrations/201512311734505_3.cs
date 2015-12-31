namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Nodes", "Likes");
            DropColumn("dbo.Nodes", "Dislikes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Nodes", "Dislikes", c => c.Int(nullable: false));
            AddColumn("dbo.Nodes", "Likes", c => c.Int(nullable: false));
        }
    }
}
