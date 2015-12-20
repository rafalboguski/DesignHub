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
        public ICollection<Node> GetNodes()
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

        [Route("{id}/image")]
        public byte[] GetNodeImage(int id)
        {
            var img = _repo.Single(id).Image;

            return img;
        }

        [Route("project/{projectId}")]
        public ICollection<Node> GetProjectNodes(int projectId)
        {
            var nodes = _repo.All(projectId);

            return nodes;
        }


        [HttpPost]
        [Route("")]
        public Node Create(NodeDTO nodeDto)
        {
            if (!ModelState.IsValid && nodeDto == null)
                return null;

            var node = _repo.Create(nodeDto);

            return node;
        }

        [HttpPost]
        [Route("{id}/head")]
        public int setHead(int id)
        {
            _service.setHead(id);
            return 200;

        }


        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Move(int id, Node data)
        {

            _repo.Update(id, data);

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
            if (await _repo.UploadImage(id, Request) == true)
                return Ok();
            else
                return NotFound();
        }




    }
}