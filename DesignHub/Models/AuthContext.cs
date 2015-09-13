using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DesignHub.Models
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {

        public AuthContext() : base("AuthContext")
        {

        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }

        public DbSet<Project> Projects { get; set; }
   

    }


}