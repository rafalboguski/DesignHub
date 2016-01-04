namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        visited = c.Boolean(nullable: false),
                        Header = c.String(),
                        Content = c.String(),
                        Link = c.String(),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "Author_Id" });
            DropTable("dbo.Notifications");
        }
    }
}
