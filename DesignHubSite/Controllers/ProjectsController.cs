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
using DesignHubSite.ExtensionMethods;

namespace DesignHubSite.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {


        private readonly IRepository<Project> _repoProjects;
        private readonly IProjectService _serviceProjects;

        public ProjectsController(IRepository<Project> repo, IProjectService serviceProjects)
        {
            _repoProjects = repo;
            _serviceProjects = serviceProjects;
        }


        [Route("")]
        public ICollection<ProjectListViewModel> GetProjects()
        {
            var projects = _repoProjects.All();

            return ProjectListViewModel.Map(projects);
        }

        [Route("{id}")]
        public Project GetProject(int id)
        {
            var project = _repoProjects.Single(id);

            return project;
        }


        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Project project)
        {
            if (!ModelState.IsValid && project == null)
                return BadRequest(ModelState);

            _repoProjects.Create(project);

            return Ok();
        }


        [Route("add")]
        public IHttpActionResult GetAdd()
        {

            var sb = ApplicationDbContext.Create();
            var id = sb.CurrentUserId();

            _repoProjects.Create(new Project { Name = "sdf", Description = "sdfsfd", Owner = sb.Users.SingleOrDefault(u => u.Id == id) });

            return Ok();
        }


        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProject(int id)
        {
            return (_repoProjects.Delete(id)) ? (IHttpActionResult)Ok() : NotFound();
        }


        [HttpPost]
        [Route("{id}/image")]
        public async Task<IHttpActionResult> UploadImage(int id)
        {
            if (await _serviceProjects.UploadImage(id, Request) == true)
                return Ok();
            else
                return NotFound();
        }


        [HttpPost]
        [Route("{projectId}/inviteWatcher/{userId}")]
        public IHttpActionResult InviteWatcher(int projectId, string userId)
        {
            if (_serviceProjects.InviteWatcher(projectId, userId))
                return Ok();
            else
                return NotFound();
        }




    }
}