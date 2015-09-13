using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using DesignHub.Models;
using DesignHub.Services.Logger;


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

        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            VSlog.Write(Request.Method, Request.RequestUri);
            var list = _db.Projects.AsNoTracking().ToList();
            return Json(list);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(Project project)
        {
            VSlog.Write(Request.Method, Request.RequestUri);

            if (!ModelState.IsValid)
                return BadRequest();

            _db.Projects.Add(project);
            _db.SaveChanges();

            return Ok();
        }

    }


}