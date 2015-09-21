using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DesignHubSite.Models;

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
            return Json(_db.Projects);
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

        [HttpGet]
        [Route("dodaj")]
        public IHttpActionResult Dodaj()
        {

            var p = new Project
            {
                Name = "aaa",
                Description = "aaa"
            };

            _db.Projects.Add(p);
            _db.SaveChanges();

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
        //        // POST: api/Projects
        //        [ResponseType(typeof(Project))]
        //        public IHttpActionResult PostProject(Project project)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return BadRequest(ModelState);
        //            }
        //
        //            _db.Projects.Add(project);
        //            _db.SaveChanges();
        //
        //            return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
        //        }
        //
        //        // DELETE: api/Projects/5
        //        [ResponseType(typeof(Project))]
        //        public IHttpActionResult DeleteProject(int id)
        //        {
        //            var project = _db.Projects.Find(id);
        //            if (project == null)
        //            {
        //                return NotFound();
        //            }
        //
        //            _db.Projects.Remove(project);
        //            _db.SaveChanges();
        //
        //            return Ok(project);
        //        }
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