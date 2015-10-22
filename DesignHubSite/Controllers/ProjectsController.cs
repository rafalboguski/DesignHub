using System.Collections.Generic;
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

namespace DesignHubSite.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {


        private readonly IProjectRepository _repo;

        public ProjectsController(IProjectRepository repo)
        {
            _repo = repo;
        }


        [Route("")]
        public ICollection<Project> GetProjects()
        {

            return _repo.GetProjects();
        }

        [Route("{id}")]
        public Project GetProject(int id)
        {
            var project = _repo.GetProject(id);
       
            return project;
        }

         
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Project project)
        {
            if (!ModelState.IsValid && project == null)
                return BadRequest(ModelState);

            _repo.CreateProject(project);

            return Ok();
        }


        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProject(int id)
        {
            return (_repo.DeleteProject(id)) ? (IHttpActionResult)Ok() : NotFound();
        }


        [HttpPost]
        [Route("{id}/image")]
        public async Task<IHttpActionResult> UploadImage(int id)
        {
            if (await _repo.UploadImage(id, Request) == true)
                return Ok();
            else
                return NotFound();
        }


        [HttpPost]
        [Route("{projectId}/inviteWatcher/{userId}")]
        public IHttpActionResult InviteWatcher(int projectId, string userId)
        {
            if (_repo.InviteWatcher(projectId, userId))
                return Ok();
            else
                return NotFound();
        }



       
    }
}