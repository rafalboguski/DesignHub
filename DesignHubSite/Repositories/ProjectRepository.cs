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
using System.Data.Entity;
using System.Net.Http;
using DesignHubSite.Services;

namespace DesignHubSite.Repositories
{


    public class ProjectRepository : IRepository<Project>
    {

        private ApplicationDbContext db = ApplicationDbContext.Create();

        private readonly INodeRepository _nodeRepository;
        private readonly INotificationReposotory _notificationRepository;
        private IPermissionRepository _permissionsRepository;

        public ProjectRepository(INodeRepository nodeRepository, INotificationReposotory notyfication, IPermissionRepository permissionsRepository)
        {
            _nodeRepository = nodeRepository;
            _notificationRepository = notyfication;
            _permissionsRepository = permissionsRepository;

        }


        public Project Single(int id)
        {
            var currentUserId = db.CurrentUserId();

            var project = db.Projects
                            .Include("AssignedUsers")
                            .Include("Owner")
                            .Include("Nodes")
                            .SingleOrDefault(p => p.Id == id);

            if (project?.Owner.Id == currentUserId)
                return project;

            var permission = _permissionsRepository.GetPermission(currentUserId, id);

            if (permission == null)
                return null;

            if (permission.Readonly)
                return project;

            return null;
        }


        public List<Project> All()
        {
            var currentUserId = db.CurrentUserId();
            var projects = from p in db.Projects
                           .Include("AssignedUsers")
                           .Include("Owner")
                           .Include("Nodes")
                           //where (p.Owner.Id == currentUserId) || (p.AssignedUsers.Select(c => c.Id).Contains(currentUserId))

                           join perm in db.Permisions.Include(x=>x.Project).Include(x => x.User)
                                on p.Id equals perm.Project.Id into FullPerm
                           from perm in FullPerm.DefaultIfEmpty()

                           where (p.Owner.Id == currentUserId) || (perm.User.Id == currentUserId && perm.Readonly)

                           orderby p.Timestamp
                           select p;

            return projects.ToList();
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
                var rootNodeId = _nodeRepository.Create(new NodeDTO() { ChangeInfo = "init", ProjectId = project.Id });


                _notificationRepository.Create(new Notification
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

                _notificationRepository.Create(new Notification
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