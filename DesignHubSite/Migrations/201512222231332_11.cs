namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectRole = c.String(),
                        Readonly = c.Boolean(nullable: false),
                        Message = c.Boolean(nullable: false),
                        LikeOrDislikeChanges = c.Boolean(nullable: false),
                        AddMarkers = c.Boolean(nullable: false),
                        AcceptNodes = c.Boolean(nullable: false),
                        AcceptWholeProject = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Project_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permisions", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Permisions", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Permisions", new[] { "User_Id" });
            DropIndex("dbo.Permisions", new[] { "Project_Id" });
            DropTable("dbo.Permisions");
        }
    }
}
