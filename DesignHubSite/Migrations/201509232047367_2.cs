namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ImageName");
        }
    }
}
