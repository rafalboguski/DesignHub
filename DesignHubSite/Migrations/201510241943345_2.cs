namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Images", new[] { "Project_Id" });
            AddColumn("dbo.Projects", "Head_Id", c => c.Int());
            AddColumn("dbo.Projects", "Root_Id", c => c.Int());
            AddColumn("dbo.Versions", "Version_Id", c => c.Int());
            AddColumn("dbo.Versions", "Project_Id", c => c.Int());
            AddColumn("dbo.Versions", "Project_Id1", c => c.Int());
            CreateIndex("dbo.Projects", "Head_Id");
            CreateIndex("dbo.Projects", "Root_Id");
            CreateIndex("dbo.Versions", "Version_Id");
            CreateIndex("dbo.Versions", "Project_Id");
            CreateIndex("dbo.Versions", "Project_Id1");
            AddForeignKey("dbo.Versions", "Version_Id", "dbo.Versions", "Id");
            AddForeignKey("dbo.Versions", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.Projects", "Head_Id", "dbo.Versions", "Id");
            AddForeignKey("dbo.Projects", "Root_Id", "dbo.Versions", "Id");
            AddForeignKey("dbo.Versions", "Project_Id1", "dbo.Projects", "Id");
            DropTable("dbo.Images");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Versions", "Project_Id1", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Root_Id", "dbo.Versions");
            DropForeignKey("dbo.Projects", "Head_Id", "dbo.Versions");
            DropForeignKey("dbo.Versions", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Versions", "Version_Id", "dbo.Versions");
            DropIndex("dbo.Versions", new[] { "Project_Id1" });
            DropIndex("dbo.Versions", new[] { "Project_Id" });
            DropIndex("dbo.Versions", new[] { "Version_Id" });
            DropIndex("dbo.Projects", new[] { "Root_Id" });
            DropIndex("dbo.Projects", new[] { "Head_Id" });
            DropColumn("dbo.Versions", "Project_Id1");
            DropColumn("dbo.Versions", "Project_Id");
            DropColumn("dbo.Versions", "Version_Id");
            DropColumn("dbo.Projects", "Root_Id");
            DropColumn("dbo.Projects", "Head_Id");
            CreateIndex("dbo.Images", "Project_Id");
            AddForeignKey("dbo.Images", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
