using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DesignHubSite.Models;
using Microsoft.AspNet.Identity;

namespace DesignHubSite.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();


        [Route("")]
        public IHttpActionResult GetProjects()
        {
            var currentUserId = User.Identity.GetUserId();

            var projects = from p in _db.Projects
                      where (p.Owner.Id == currentUserId)
                      || (p.Watchers.Select(c => c.Id).Contains(currentUserId))
                      select p;
         
            return Json(projects);
        }



        [HttpPost]
        [Route("{id}/image")]
        public async Task<IHttpActionResult> UploadImageToProject(int id)
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

                project.ImageName = filename;
                project.Image = buffer;
            }

            _db.SaveChanges();
            return Ok();
        }


        // POST: api/Projects
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateProject(Project project)
        {
            if (!ModelState.IsValid && project == null)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);

            currentUser.Projects.Add(project);
            project.Owner = currentUser;

            _db.Projects.Add(project);
            _db.SaveChanges();

            return Ok();
        }


        // DELETE: api/Projects/5
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProject(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Watchers.Clear();

            _db.Projects.Remove(project);
            _db.SaveChanges();

            return Ok();
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




        //        [ResponseType(typeof(Project))]
        //        public IHttpActionResult GetProject(int id)
        //        {
        //            var project = _db.Projects.Find(id);
        //            if (project == null)
        //            {
        //                return NotFound();
        //            }
        //
        //            return Json("sdfsdf");
        //        }



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




        //
        //        protected override void Dispose(bool disposing)
        //        {
        //            if (disposing)
        //            {
        //                _db.Dispose();
        //            }
        //            base.Dispose(disposing);
        //        }
        //
        //        private bool ProjectExists(int id)
        //        {
        //            return _db.Projects.Count(e => e.Id == id) > 0;
        //        }
    }
}