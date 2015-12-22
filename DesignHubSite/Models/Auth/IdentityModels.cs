using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using DesignHubSite.Models;

namespace DesignHubSite.Models
{


    public interface IApplicationDbContext
    {
        DbSet<Project> Projects { get; set; }


        int SaveChanges();
        Database Database { get; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            var context = new ApplicationDbContext();
            context.Configuration.LazyLoadingEnabled = false;
            return context;
        }



        public DbSet<Project> Projects { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Marker> Markers { get; set; }
        public DbSet<Permision> Permisions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //one-to-many 
            //modelBuilder.Entity<Project>()
            //            .HasRequired<ApplicationUser>(s => s.Owner)
            //            .WithMany(s => s.Projects)
            //            .HasForeignKey(s => s.OwnerId);

            modelBuilder.Entity<Project>()
                        .HasMany(x => x.AssignedUsers)
                        .WithMany(x => x.AssignedProjects)
                        .Map(x =>
                        {
                            x.ToTable("ProjectsAndWatchers"); // third table is named Cookbooks
                            x.MapLeftKey("ProjectId");
                            x.MapRightKey("UserId");
                        })
                        ;
        }
    }
}

namespace DesignHubSite.ExtensionMethods
{

    public static class MyExtensionMethods
    {

        public static string CurrentUserId(this ApplicationDbContext context)
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

    }

}