using Microsoft.AspNet.Identity.EntityFramework;

namespace DesignHub.Models
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {

        public AuthContext() : base("AuthContext")
        {

        }

    }
}