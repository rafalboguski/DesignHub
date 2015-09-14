using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using DesignHub.Models;
using DesignHub.Services.Logger;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace DesignHub.Controllers
{
    [RoutePrefix("api/Projects")]
    public class ProjectController : ApiController
    {

        private readonly AuthContext _db;

        public ProjectController()
        {
            _db = AuthContext.Create();
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            VSlog.Write(Request.Method, Request.RequestUri);

            var list = _db.Projects.AsNoTracking().ToList();

            return Json(list);
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public IHttpActionResult Post(Project project)
        {
            VSlog.Write(Request.Method, Request.RequestUri);

            if (!ModelState.IsValid)
                return BadRequest();

           
            var currentUser = GetCurrentUser(Request);

            project.Owner = currentUser;
            _db.Projects.Add(project);
            currentUser.Projects.Add(project);
            
                                        
            
            _db.SaveChanges();

            return Ok();
        }

        // to services

        public MyUser GetCurrentUser(HttpRequestMessage request)
        {
            var principal = request.GetRequestContext().Principal as ClaimsPrincipal;

            var name = principal?.Claims.Single(c => c.Type == "sub").Value;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_db));
            var currentUser = userManager.FindByName(name);

            return (MyUser)currentUser;
        }


    }


}