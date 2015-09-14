namespace DesignHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Owner_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "Owner_Id");
            AddForeignKey("dbo.Projects", "Owner_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Projects", "Owner_UserName");
            DropColumn("dbo.Projects", "Owner_Password");
            DropColumn("dbo.Projects", "Owner_ConfirmPassword");
            DropColumn("dbo.AspNetUsers", "Aaaaaa");
            DropColumn("dbo.AspNetUsers", "Aaaaab");
            DropColumn("dbo.AspNetUsers", "Aaaaac");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Aaaaac", c => c.String());
            AddColumn("dbo.AspNetUsers", "Aaaaab", c => c.String());
            AddColumn("dbo.AspNetUsers", "Aaaaaa", c => c.String());
            AddColumn("dbo.Projects", "Owner_ConfirmPassword", c => c.String());
            AddColumn("dbo.Projects", "Owner_Password", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Projects", "Owner_UserName", c => c.String(nullable: false));
            DropForeignKey("dbo.Projects", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "Owner_Id" });
            DropColumn("dbo.Projects", "Owner_Id");
        }
    }
}
