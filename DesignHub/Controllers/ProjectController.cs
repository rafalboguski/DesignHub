using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DesignHub.Services.Logger;


namespace DesignHub.Controllers
{
    [RoutePrefix("api/Projects")]
    public class ProjectController : ApiController
    {

        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            VSlog.Write(Request.Method, Request.RequestUri);
            var list = Project.CreateProjects();
            return Json(list);
        }

    }

    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public bool Finished { get; set; }

        public Project(int projectId, string name, bool finished)
        {
            ProjectID = projectId;
            Name = name;
            Finished = finished;
        }


        public static List<Project> CreateProjects()
        {
            var list = new List<Project>
            {
              new Project(10001, "Ziemniak", false),
              new Project(10002, "Kartofel", true)
            };
            return list;
        }
    }
}