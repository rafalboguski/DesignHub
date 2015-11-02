namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nodes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Versions", newName: "Nodes");
            RenameColumn(table: "dbo.Nodes", name: "Version_Id", newName: "Father_Id");
            RenameColumn(table: "dbo.Nodes", name: "Project_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Nodes", name: "Project_Id1", newName: "Project_Id");
            RenameColumn(table: "dbo.Nodes", name: "__mig_tmp__0", newName: "Project_Id1");
            RenameIndex(table: "dbo.Nodes", name: "IX_Version_Id", newName: "IX_Father_Id");
            RenameIndex(table: "dbo.Nodes", name: "IX_Project_Id1", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Nodes", name: "IX_Project_Id", newName: "IX_Project_Id1");
            RenameIndex(table: "dbo.Nodes", name: "__mig_tmp__0", newName: "IX_Project_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Nodes", name: "IX_Project_Id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Nodes", name: "IX_Project_Id1", newName: "IX_Project_Id");
            RenameIndex(table: "dbo.Nodes", name: "__mig_tmp__0", newName: "IX_Project_Id1");
            RenameIndex(table: "dbo.Nodes", name: "IX_Father_Id", newName: "IX_Version_Id");
            RenameColumn(table: "dbo.Nodes", name: "Project_Id1", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Nodes", name: "Project_Id", newName: "Project_Id1");
            RenameColumn(table: "dbo.Nodes", name: "__mig_tmp__0", newName: "Project_Id");
            RenameColumn(table: "dbo.Nodes", name: "Father_Id", newName: "Version_Id");
            RenameTable(name: "dbo.Nodes", newName: "Versions");
        }
    }
}
