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
    [RoutePrefix("api/Nodes")]
    public class NodesController : ApiController
    {


        private readonly INodeRepository _repo;
        private readonly IProjectService _service;

        public NodesController(INodeRepository repo, IProjectService service)
        {
            _repo = repo;
            _service = service;
        }


        [Route("")]
        public ICollection<Node> GetProjects()
        {
            var nodes = _repo.All();

            return nodes;
        }

        [Route("{id}")]
        public Node GetNodes(int id)
        {
            var node = _repo.Single(id);

            return node;
        }


        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Node node, int projectId, int? fatherNodeId = null)
        {
            if (!ModelState.IsValid && node == null)
                return BadRequest(ModelState);

            _repo.Create(node, projectId, fatherNodeId);

            return Ok();
        }




        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProject(int id)
        {
            return (_repo.Delete(id)) ? (IHttpActionResult)Ok() : NotFound();
        }


        [HttpPost]
        [Route("{id}/image")]
        public async Task<IHttpActionResult> UploadImage(int id)
        {
            if (await _service.UploadImage(id, Request) == true)
                return Ok();
            else
                return NotFound();
        }




    }
}