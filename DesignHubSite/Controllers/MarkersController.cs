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
    [RoutePrefix("api/Markers")]
    public class MarkersController : ApiController
    {


        private readonly IMarkerRepository _repo;     

        public MarkersController(IMarkerRepository repo)
        {
            _repo = repo;
           
        }


        [Route("node/{id}")]
        public ICollection<Marker> GetMarkers(int id)
        {
            var markers = _repo.All(id);

            return markers;
        }

        [Route("{id}")]
        public Marker GetMarker(int id)
        {
            var marker = _repo.Single(id);

            return marker;
        }


        [HttpPost]
        [Route("")]
        public Marker Create(MarkerDto dto)
        {
            if (!ModelState.IsValid && dto == null)
                return null;

            var marker = _repo.Create(dto);

            return marker;
        }


        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteMarker(int id)
        {
            return (_repo.Delete(id)) ? (IHttpActionResult)Ok() : NotFound();
        }


    }
}