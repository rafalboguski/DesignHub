namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarkerOpinions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Opinion = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Marker_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Markers", t => t.Marker_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Marker_Id);
            
            AddColumn("dbo.Markers", "Number", c => c.Int(nullable: false));
            DropColumn("dbo.Markers", "text");
            DropColumn("dbo.Markers", "Timestamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Markers", "Timestamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Markers", "text", c => c.String());
            DropForeignKey("dbo.MarkerOpinions", "Marker_Id", "dbo.Markers");
            DropForeignKey("dbo.MarkerOpinions", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.MarkerOpinions", new[] { "Marker_Id" });
            DropIndex("dbo.MarkerOpinions", new[] { "Author_Id" });
            DropColumn("dbo.Markers", "Number");
            DropTable("dbo.MarkerOpinions");
        }
    }
}
