using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace DesignHubSite.Models
{



    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }



        public DbSet<Project> Projects { get; set; }

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
                        .HasMany(x => x.Watchers)
                        .WithMany(x => x.WatchedProjects)
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