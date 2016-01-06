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

namespace DesignHubSite.Repositories
{


    public class ProjectRepository : IRepository<Project>
    {

        private readonly INodeRepository _nodeRepo;
        private readonly INotificationReposotory _notyfication;

        public ProjectRepository(INodeRepository nodeRepository, INotificationReposotory notyfication)
        {
            _nodeRepo = nodeRepository;
            _notyfication = notyfication;

        }

        public Project Single(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {

                db.Configuration.LazyLoadingEnabled = false;

                var currentUserId = db.CurrentUserId();


                var prop = db.Projects.Where(x => x.Id == id).ToList();

                var pa = db.Projects
                                .Include("AssignedUsers")
                                .Include("Owner")
                                .Include("Nodes")
                                .SingleOrDefault(p => 
                                    (p.Id == id) 
                                    &&
                                    (
                                        (p.Owner.Id == currentUserId) 
                                        || 
                                        (p.AssignedUsers.Select(c => c.Id).Contains(currentUserId))
                                    )
                                );

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
                               .Include("AssignedUsers")
                               .Include("Owner")
                               .Include("Nodes")
                               where (p.Owner.Id == currentUserId) || (p.AssignedUsers.Select(c => c.Id).Contains(currentUserId))
                               orderby p.Timestamp
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

                project.Owner = currentUser;
                project.CreateDate = DateTime.Now;
                project.Timestamp = DateTime.Now;

                db.Projects.Add(project);
                db.SaveChanges();

                currentUser.OwnedProjects.Add(project);
                db.SaveChanges();

                // add initial node
                var rootNodeId = _nodeRepo.Create(new NodeDTO() { ChangeInfo = "init", ProjectId = project.Id });


                _notyfication.Create(new Notification
                {
                    Author = currentUser,
                    Header = "Project created",
                    Priority = 1,
                    ProjectId = project.Id,
                    Link = null
                });
            }
        }

        public bool Delete(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var project = db.Projects
                    .Include("Owner")
                    .Include("Nodes")
                    .SingleOrDefault(x => x.Id == id);


                if (project == null)
                {
                    return false;
                }
                if (project.Owner.Id != currentUserId)
                {
                    return false;
                }

                project.AssignedUsers.Clear();

                foreach (var version in project.Nodes)
                    version.Project = null;

                project.Nodes.Clear();

                db.Projects.Remove(project);
                db.SaveChanges();

                _notyfication.Create(new Notification
                {
                    Author = db.Users.Single(x => x.Id == currentUserId),
                    Header = "Project deleted",
                    Priority = 1,
                    ProjectId = project.Id,
                    Link = null
                });
                return true;
            }
        }


    }


}