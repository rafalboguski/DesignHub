using System;
using System.Collections.Generic;
using System.Linq;
using DesignHubSite.Models;
using Microsoft.VisualBasic.ApplicationServices;
using System.Web;
using Microsoft.AspNet.Identity;
using DesignHubSite.ExtensionMethods;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http;

namespace DesignHubSite.Services
{

    public interface IProjectRepository
    {

        Project GetProject(int id);

        List<Project> GetProjects();

        void CreateProject(Project project);

        bool DeleteProject(int id);

        Task<bool> UploadImage(int id, HttpRequestMessage request);

        bool InviteWatcher(int projectId, string userId);

    }


    public class ProjectRepository : IProjectRepository
    {


        public Project GetProject(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
            
                var pa = db.Projects.Include("Watchers").Include("Owner").Include("Versions").Include("Head")
                               .Include("Root")
                                .SingleOrDefault(p => (p.Id == id) &&
                                (p.Owner.Id == currentUserId) || (p.Watchers.Select(c => c.Id).Contains(currentUserId)));

                return pa;
            }
        }


        public List<Project> GetProjects()
        {
            using (var db = ApplicationDbContext.Create())
            {

                var currentUserId = HttpContext.Current.User.Identity.GetUserId();
                var projects = from p in db.Projects
                               .Include("Watchers")
                               .Include("Owner")
                               .Include("Versions")
                               .Include("Head")
                               .Include("Root")
                               where (p.Owner.Id == currentUserId) || (p.Watchers.Select(c => c.Id).Contains(currentUserId))
                               select p;

                

                return projects.ToList();
            }
        }

        public void CreateProject(Project project)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                currentUser.Projects.Add(project);
                project.Owner = currentUser;

                db.Projects.Add(project);
                db.SaveChanges();
            }
        }

        public bool DeleteProject(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var project = db.Projects.Find(id);


                if (project == null)
                {
                    return false;
                }
                if (project.Owner.Id != currentUserId)
                {
                    return false;
                }

                project.Watchers.Clear();

                foreach (var version in project.Versions)
                    version.Project = null;

                project.Versions.Clear();

                db.Projects.Remove(project);
                db.SaveChanges();

                return true;
            }
        }

        public async Task<bool> UploadImage(int id, HttpRequestMessage request)
        {
            if (!request.Content.IsMimeMultipartContent())
                return false;

            using (var db = ApplicationDbContext.Create())
            {
                // TODO: check if right owner
                var project = db.Projects.Single(x => x.Id == id);

                if (project == null)
                    return false;

                var provider = new MultipartMemoryStreamProvider();
                await request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();

                    //                project.ImageName = filename;
                    //                project.Image = buffer;
                }

                db.SaveChanges();
                return true;

            }
        }

        public bool InviteWatcher(int projectId, string userId)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var project = db.Projects.First(p => p.Id == projectId);
                var user = db.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null && project != null && project.Owner.Id == db.CurrentUserId())
                {
                    project.Watchers.Add(user);
                    user.WatchedProjects.Add(project);

                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



    }


}