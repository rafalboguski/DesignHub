namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notifications", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "Content", c => c.String());
        }
    }
}
