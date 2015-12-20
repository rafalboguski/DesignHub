namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "nodeHeadId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "nodeHeadId");
        }
    }
}
