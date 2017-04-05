using DesignHubSite.Models;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace DesignHubSite.Services
{

    public interface IUsersService
    {
        string LoggedUserId();
        ApplicationUser LoggedUser();
    }


    public class UsersService : IUsersService
    {

        private ApplicationDbContext _db = ApplicationDbContext.Create();


        public ApplicationUser LoggedUser()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            var loggedUserId = LoggedUserId();
            var user = _db.Users.Include("OwnedProjects.Owner")
                                .Include("OwnedProjects.AssignedUsers")
                                .Include("AssignedProjects.Owner")
                                .Include("AssignedProjects.AssignedUsers")
                                .Single(x => x.Id == loggedUserId)
                                ;
          
            return user;
        }

        public string LoggedUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }

}