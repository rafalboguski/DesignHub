namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarkerOpinionReplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Opinion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.MarkerOpinions", t => t.Opinion_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Opinion_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MarkerOpinionReplies", "Opinion_Id", "dbo.MarkerOpinions");
            DropForeignKey("dbo.MarkerOpinionReplies", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.MarkerOpinionReplies", new[] { "Opinion_Id" });
            DropIndex("dbo.MarkerOpinionReplies", new[] { "Author_Id" });
            DropTable("dbo.MarkerOpinionReplies");
        }
    }
}
