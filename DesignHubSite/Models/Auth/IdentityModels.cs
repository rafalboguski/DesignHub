using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using DesignHubSite.Models;
using System;
using Microsoft.Practices.Unity;

namespace DesignHubSite.Models
{


    public interface IApplicationDbContext<TUser> where TUser : class
    {


        DbSet<Project> Projects { get; }
        DbSet<ProjectNote> ProjectsNotes { get; set; }
        DbSet<Node> Nodes { get; set; }
        DbSet<Marker> Markers { get; set; }
        DbSet<MarkerOpinion> MarkersOpinions { get; set; }
        DbSet<MarkerOpinionReply> MarkersOpinionsReplies { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Permision> Permisions { get; set; }
        IDbSet<TUser> Users { get; set; }

        int SaveChanges();

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        internal object Single(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }

        public static ApplicationDbContext Create()
        {
            //var context = MvcApplication.Container.Resolve<ApplicationDbContext>();
            var context = new ApplicationDbContext();
            context.Configuration.LazyLoadingEnabled = false;
            return context;
        }



        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectNote> ProjectsNotes { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Marker> Markers { get; set; }
        public DbSet<MarkerOpinion> MarkersOpinions { get; set; }
        public DbSet<MarkerOpinionReply> MarkersOpinionsReplies { get; set; }
        public DbSet<Notification> Notifications { get; set; }
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


        public static string CurrentUserId(this IApplicationDbContext<ApplicationUser> context)
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }



    }

}