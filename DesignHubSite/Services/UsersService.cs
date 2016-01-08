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

        private IApplicationDbContext<ApplicationUser> _db;

        public UsersService(IApplicationDbContext<ApplicationUser> db)
        {
            _db = db;
        }

        public ApplicationUser LoggedUser()
        {
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