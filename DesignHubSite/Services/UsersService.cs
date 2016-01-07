using DesignHubSite.Models;
using DesignHubSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DesignHubSite.ExtensionMethods;
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
            var loggedUserId = LoggedUserId();
            var user = _db.Users.Include("Permissions.Project.Owner")
                                .Include(x => x.OwnedProjects)
                                .Include(x => x.AssignedProjects)
                                .Single(x => x.Id == loggedUserId);
            return user;
        }

        public string LoggedUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }

}