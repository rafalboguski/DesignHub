namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSerializer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Projects", "Description", c => c.String(maxLength: 400));
            DropColumn("dbo.Projects", "ImageName");
            DropColumn("dbo.Projects", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "Image", c => c.Binary());
            AddColumn("dbo.Projects", "ImageName", c => c.String());
            AlterColumn("dbo.Projects", "Description", c => c.String());
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false));
        }
    }
}
