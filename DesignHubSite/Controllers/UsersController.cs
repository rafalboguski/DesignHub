using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DesignHubSite.Models;
using DesignHubSite.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DesignHubSite.ExtensionMethods;
using DesignHubSite.Services;
using System.Data.Entity;
using System;

namespace DesignHubSite.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {


        private IPermissionRepository _permissionsRepository;
        private IProjectDetailsService _projDetServ;

        public UsersController(IPermissionRepository permissionsRepository, IProjectDetailsService projectDetailsService)
        {
            _permissionsRepository = permissionsRepository;
            _projDetServ = projectDetailsService;
        }


        [Route("")]
        public ICollection<ApplicationUser> GetUsers()
        {
            using (var db = ApplicationDbContext.Create())
            {
                return db.Users.ToList();
            }

        }

        // Returns list of known users 
        // if you own project => all users assigned to it
        // if you are assigned to project => owner
        [Route("contacts")]
        public IHttpActionResult GetContacts()
        {
            using (var db = ApplicationDbContext.Create())
            {
                var loggedUserId = db.CurrentUserId();

                //////////////////////////////////////////////////////

                var createdProjectsId = db.Projects
                                         .Include(x => x.Owner)
                                         .Where(x => x.Owner.Id == loggedUserId).Select(x => x.Id);

                List<string> assignedId = db.Permisions
                                    .Include(X => X.User)
                                    .Include(X => X.Project)
                                    .Where(x => createdProjectsId.Contains(x.Project.Id))
                                    .Select(x => x.User.Id)
                                    .Distinct()
                                    .ToList();

                var assigned = new List<Tuple<ApplicationUser, List<Tuple<int, string>>>>();

                assignedId.ForEach(userId =>
                {

                    var e = new Tuple<ApplicationUser, List<Tuple<int, string>>>(
                           db.Users.Single(x => x.Id == userId),
                           db.Permisions.Include(X => X.User)
                                        .Include(X => X.Project)
                                        .Where(x => x.User.Id == userId)
                                        .ToList()
                                        .Select(x => new Tuple<int, string>(x.Project.Id, x.Project.Name))
                                        .ToList()
                        );

                    assigned.Add(e);

                });

                ////////////////////////////////////////////////

                List<string> ownersId = db.Permisions
                                   .Include(X => X.User)
                                   .Include(X => X.Project)
                                   .Where(x => x.User.Id == loggedUserId)
                                   .Select(x => x.Project.Owner.Id)
                                   .Distinct()
                                   .ToList();

                var owners = new List<Tuple<ApplicationUser, List<Tuple<int, string>>>>();

                ownersId.ForEach(userId =>
                {
                var e = new Tuple<ApplicationUser, List<Tuple<int, string>>>(
                       db.Users.Single(x => x.Id == userId),
                       db.Projects.Include(X => X.Owner)
                                  .Where(x => x.Owner.Id == userId)
                                  .ToList()
                                  .Select(x => new Tuple<int, string>(x.Id, x.Name))
                                  .ToList()

                        );

                owners.Add(e);

            });

            return Json(new { Developers = owners, Customers = assigned });
        }
    }

    [Route("{id}")]
    public ApplicationUser GetUser(string id)
    {
        using (var db = ApplicationDbContext.Create())
        {
            return db.Users.SingleOrDefault(x => x.Id == id);
        }

    }

    [Route("find/{text}")]
    public ICollection<ApplicationUser> GetUsersLike(string text)
    {
        using (var db = ApplicationDbContext.Create())
        {
            var loggedUserId = db.CurrentUserId();

            var list = db.Users
                .Where(x =>
                (x.UserName.Contains(text) || x.Email.Contains(text))
                && (x.Id != loggedUserId)
                )
                .Take(9);

            return list.ToList();
        }

    }

    [Route("permissions_in_project/{projectId}")]
    public List<Permision> getPermissionsInProject(int projectId)
    {
        var list = _permissionsRepository.GetPermissions(projectId: projectId);

        return list;

    }


    [HttpPost]
    [Route("assignToProject")]
    public IHttpActionResult AssignToProject(PermisionDto dto)
    {

        if (_projDetServ.InviteUserToProject(dto))
        {
            return Ok();
        }
        else
        {
            return NotFound();
        }

    }

}
}