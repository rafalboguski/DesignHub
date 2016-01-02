namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "nodeHeadId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "nodeHeadId", c => c.Int(nullable: false));
        }
    }
}
