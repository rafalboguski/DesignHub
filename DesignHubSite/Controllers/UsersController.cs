﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DesignHubSite.Models;
using DesignHubSite.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DesignHubSite.ExtensionMethods;

namespace DesignHubSite.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {


        private IProjectDetailsService _projDetServ;

        public UsersController(IProjectDetailsService projectDetailsService)
        {
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
                var list = db.Users
                    .Where(x => x.UserName.Contains(text) || x.Email.Contains(text))
                    .Take(9);

                return list.ToList();
            }

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