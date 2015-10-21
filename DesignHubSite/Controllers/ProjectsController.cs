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
        public IHttpActionResult Projects()
        {

            return Json(_repo.GetProjects());
        }

        [Route("{id}")]
        public IHttpActionResult Project(int id)
        {
            var project = _repo.GetProject(id);
            if (_repo.GetProject(id) == null)
            {
                return NotFound();
            }

            return Json(project);
        }



        [HttpPost]
        [Route("{id}/image")]
        public async Task<IHttpActionResult> UploadImage(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var project = _db.Projects.Single(x => x.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();

                //                project.ImageName = filename;
                //                project.Image = buffer;
            }

            _db.SaveChanges();
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Project project)
        {
            if (!ModelState.IsValid && project == null)
            {
                return BadRequest(ModelState);
            }

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
        [Route("{projectId}/inviteWatcher/{userId}")]
        public IHttpActionResult InviteWatcher(int projectId, string userId)
        {
            var currentUserId = User.Identity.GetUserId();

            var project = _db.Projects.First(p => p.Id == projectId);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null && project != null && project.Owner.Id == currentUserId)
            {
                project.Watchers.Add(user);
                user.WatchedProjects.Add(project);

                _db.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }








        //        // PUT: api/Projects/5
        //        [ResponseType(typeof(void))]
        //        public IHttpActionResult PutProject(int id, Project project)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return BadRequest(ModelState);
        //            }
        //
        //            if (id != project.Id)
        //            {
        //                return BadRequest();
        //            }
        //
        //            _db.Entry(project).State = EntityState.Modified;
        //
        //            try
        //            {
        //                _db.SaveChanges();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!ProjectExists(id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //
        //            return StatusCode(HttpStatusCode.NoContent);
        //        }
        //



        //[HttpPost]
        //[Route("{id}/image")]
        //public HttpResponseMessage SendImage(int id)
        //{
        //    var httpRequest = HttpContext.Current.Request;
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        foreach (string file in httpRequest.Files)
        //        {
        //            var postedFile = httpRequest.Files[file];
        //            var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
        //            postedFile.SaveAs(filePath);
        //            // NOTE: To store in memory use postedFile.InputStream
        //        }

        //        return Request.CreateResponse(HttpStatusCode.Created);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.BadRequest);
        //}




        
    }
}