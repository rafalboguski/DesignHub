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
        private readonly IProjectService _serviceProjects;

        private readonly INodeRepository _repoNodes;

        public ProjectsController(
            IRepository<Project> repo,
            INodeRepository nodesRepo,
            IProjectService serviceProjects)
        {
            _repoProjects = repo;
            _repoNodes = nodesRepo;
            _serviceProjects = serviceProjects;
        }


        [Route("")]
        public ICollection<ProjectListViewModel> GetProjects()
        {
            var projects = _repoProjects.All();

            var dtoList = ProjectListViewModel.Map(projects);

            if (dtoList.Count > 0)
                dtoList.ForEach(x => x.HeadImage = _repoNodes.Single(x.HeadNodeId)?.Image);

            return dtoList;
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
        [Route("{id}/status/{status}")]
        public HttpResponseMessage ChangeStatus(int id, string status)
        {
            List<string> errors;
            if (status == "accept")
            {
                _serviceProjects.AcceptProject(id, out errors);
            }
            else if (status == "reject")
            {
                _serviceProjects.RejectProject(id, out errors);
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



        //[Route("{id}")]
        //public IHttpActionResult getWatchers(int id)
        //{
        //    return (_repoProjects.Delete(id)) ? (IHttpActionResult)Ok() : NotFound();
        //}



    }
}