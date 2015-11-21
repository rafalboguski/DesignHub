namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nodes", "positionX", c => c.Int(nullable: false));
            AddColumn("dbo.Nodes", "positionY", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Nodes", "positionY");
            DropColumn("dbo.Nodes", "positionX");
        }
    }
}
