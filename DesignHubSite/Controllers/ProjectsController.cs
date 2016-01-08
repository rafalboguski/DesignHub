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

namespace DesignHubSite.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {


        private readonly IRepository<Project> _repoProjects;
        private readonly IProjectService _projectsService;

        private readonly INodeRepository _repoNodes;

        public ProjectsController(
            IRepository<Project> repo,
            INodeRepository nodesRepo,
            IProjectService serviceProjects)
        {
            _repoProjects = repo;
            _repoNodes = nodesRepo;
            _projectsService = serviceProjects;
        }


        [Route("")]
        public ICollection<ProjectListViewModel> GetProjects()
        {
            var projects = _projectsService.GetLoggedUserVisibleProjects();

            var dtoList = ProjectListViewModel.Map(projects);

            if (dtoList.Count > 0)
                dtoList.ForEach(x => x.HeadImage = _repoNodes.Single(x.HeadNodeId)?.Thumbnail);
            _projectsService.Test();

            return dtoList;
        }

        [Route("{id}")]
        public Project GetProject(int id)
        {
            var project = _projectsService.GetLoggedUserVisibleProject(id);

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


        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProject(int id)
        {
            return (_repoProjects.Delete(id)) ? (IHttpActionResult)Ok() : NotFound();
        }

        [HttpPost]
        [Route("{id}/status/{status}")]
        public HttpResponseMessage ChangeStatus(int id, string status)
        {
            List<string> errors;
            if (status == "accept")
            {
                _projectsService.AcceptProject(id, out errors);
            }
            else if (status == "reject")
            {
                _projectsService.RejectProject(id, out errors);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            if (errors.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, errors);
            }



            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [Route("{id}/notes")]
        public List<ProjectNote> GetNotes(int id)
        {
            return _projectsService.GetNotes(id);
        }

        [HttpPost]
        [Route("{id}/notes")]
        public ProjectNote AddNote(int id, dynamic data)
        {
            string note = data.text;
            return _projectsService.AddNote(id, note);
        }

        [HttpDelete]
        [Route("{projectId}/notes/{id}")]
        public IHttpActionResult DeleteNote(int projectId, int id)
        {
            _projectsService.RemoveNote(id);
            return Ok();
        }

        //[Route("{id}")]
        //public IHttpActionResult getWatchers(int id)
        //{
        //    return (_repoProjects.Delete(id)) ? (IHttpActionResult)Ok() : NotFound();
        //}



    }
}