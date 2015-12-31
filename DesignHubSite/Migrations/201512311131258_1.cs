namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Markers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        text = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Node_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nodes", t => t.Node_Id)
                .Index(t => t.Node_Id);
            
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChangeInfo = c.String(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Image = c.Binary(),
                        Root = c.Boolean(nullable: false),
                        Head = c.Boolean(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                        Rejected = c.Boolean(nullable: false),
                        Likes = c.Int(nullable: false),
                        Dislikes = c.Int(nullable: false),
                        positionX = c.Int(nullable: false),
                        positionY = c.Int(nullable: false),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        Description = c.String(maxLength: 4000),
                        nodeHeadId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.NodeNodes",
                c => new
                    {
                        Node_Id = c.Int(nullable: false),
                        Node_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Node_Id, t.Node_Id1 })
                .ForeignKey("dbo.Nodes", t => t.Node_Id)
                .ForeignKey("dbo.Nodes", t => t.Node_Id1)
                .Index(t => t.Node_Id)
                .Index(t => t.Node_Id1);
            
            CreateTable(
                "dbo.ProjectsAndWatchers",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.UserId })
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Permisions", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Permisions", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Nodes", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectsAndWatchers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectsAndWatchers", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Markers", "Node_Id", "dbo.Nodes");
            DropForeignKey("dbo.NodeNodes", "Node_Id1", "dbo.Nodes");
            DropForeignKey("dbo.NodeNodes", "Node_Id", "dbo.Nodes");
            DropIndex("dbo.ProjectsAndWatchers", new[] { "UserId" });
            DropIndex("dbo.ProjectsAndWatchers", new[] { "ProjectId" });
            DropIndex("dbo.NodeNodes", new[] { "Node_Id1" });
            DropIndex("dbo.NodeNodes", new[] { "Node_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Permisions", new[] { "User_Id" });
            DropIndex("dbo.Permisions", new[] { "Project_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Projects", new[] { "Owner_Id" });
            DropIndex("dbo.Nodes", new[] { "Project_Id" });
            DropIndex("dbo.Markers", new[] { "Node_Id" });
            DropTable("dbo.ProjectsAndWatchers");
            DropTable("dbo.NodeNodes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Permisions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Projects");
            DropTable("dbo.Nodes");
            DropTable("dbo.Markers");
        }
    }
}
