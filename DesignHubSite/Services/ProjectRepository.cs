using System;
using System.Collections.Generic;
using System.Linq;
using DesignHubSite.Models;
using Microsoft.VisualBasic.ApplicationServices;
using System.Web;
using Microsoft.AspNet.Identity;
using DesignHubSite.ExtensionMethods;

namespace DesignHubSite.Services
{

    public interface IProjectRepository
    {

        Project GetProject(int id);

        List<Project> GetProjects();

        void CreateProject(Project project);

        bool DeleteProject(int id);

    }


    public class ProjectRepository : IProjectRepository
    {


        public Project GetProject(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
            
                var pa = db.Projects.Include("Watchers").Include("Owner")
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

                db.Projects.Remove(project);
                db.SaveChanges();

                return true;
            }
        }

        
    }


}