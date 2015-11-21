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


    public class ProjectRepository : IRepository<Project>
    {

        private readonly INodeRepository _nodeRepo;

        public ProjectRepository(INodeRepository nodeRepository)
        {
            _nodeRepo = nodeRepository;

        }

        public Project Single(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {

                db.Configuration.LazyLoadingEnabled = false;

                var currentUserId = db.CurrentUserId();

                var pa = db.Projects
                                .Include("Watchers")
                                .Include("Owner")
                                .Include("Nodes")
                                .SingleOrDefault(p => (p.Id == id) &&
                                (p.Owner.Id == currentUserId) || (p.Watchers.Select(c => c.Id).Contains(currentUserId)));

                return pa;
            }
        }


        public List<Project> All()
        {
            using (var db = ApplicationDbContext.Create())
            {

                db.Configuration.LazyLoadingEnabled = false;

                var currentUserId = HttpContext.Current.User.Identity.GetUserId();
                var projects = from p in db.Projects
                               .Include("Watchers")
                               .Include("Owner")
                               .Include("Nodes")
                               where (p.Owner.Id == currentUserId) || (p.Watchers.Select(c => c.Id).Contains(currentUserId))
                               select p;



                return projects.ToList();
            }
        }

        public void Create(Project project)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                currentUser.Projects.Add(project);
                project.Owner = currentUser;

                db.Projects.Add(project);
                db.SaveChanges();

                // add initial node
                var rootNodeId = _nodeRepo.Create(new Node() { ChangeInfo = "init" }, project.Id, null);


            }
        }

        public bool Delete(int id)
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

                foreach (var version in project.Nodes)
                    version.Project = null;

                project.Nodes.Clear();

                db.Projects.Remove(project);
                db.SaveChanges();

                return true;
            }
        }


    }


}