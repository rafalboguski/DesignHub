namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "Owner_Id", "dbo.AspNetUsers");
            AddColumn("dbo.Projects", "Accepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Projects", "Rejected", c => c.Boolean(nullable: false));
            AddColumn("dbo.Projects", "DurationDays", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "EndDate", c => c.DateTime());
            AddColumn("dbo.Projects", "WhoAccepted_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Projects", "WhoRejected_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Projects", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "WhoAccepted_Id");
            CreateIndex("dbo.Projects", "WhoRejected_Id");
            CreateIndex("dbo.Projects", "ApplicationUser_Id");
            AddForeignKey("dbo.Projects", "WhoAccepted_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Projects", "WhoRejected_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "WhoRejected_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "WhoAccepted_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Projects", new[] { "WhoRejected_Id" });
            DropIndex("dbo.Projects", new[] { "WhoAccepted_Id" });
            DropColumn("dbo.Projects", "ApplicationUser_Id");
            DropColumn("dbo.Projects", "WhoRejected_Id");
            DropColumn("dbo.Projects", "WhoAccepted_Id");
            DropColumn("dbo.Projects", "EndDate");
            DropColumn("dbo.Projects", "DurationDays");
            DropColumn("dbo.Projects", "Rejected");
            DropColumn("dbo.Projects", "Accepted");
            AddForeignKey("dbo.Projects", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
