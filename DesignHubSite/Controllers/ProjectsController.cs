using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
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
            System.Diagnostics.Debug.WriteLine("ProjectsController:GetProjects()");
            return Json(_db.Projects.AsNoTracking());
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

                project.ImageName = filename;
                project.Image = buffer;
            }

            _db.SaveChanges();
            return Ok();
        }




        // POST: api/Projects
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid && project == null)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);

            project.OwnerId = currentUserId;

            _db.Projects.Add(project);


            currentUser.Projects.Add(project);


            _db.SaveChanges();

            return Json(project.Id);
        }

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

            _db.Projects.Remove(project);
            _db.SaveChanges();

            return Ok();
        }



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