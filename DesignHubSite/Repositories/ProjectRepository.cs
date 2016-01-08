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
using System.Transactions;
using DesignHubSite.Services;

namespace DesignHubSite.Repositories
{


    public class ProjectRepository : IRepository<Project>
    {

        private IApplicationDbContext<ApplicationUser> db;

        private readonly INodeRepository _nodeRepository;
        private readonly INotificationReposotory _notificationRepository;
        private IPermissionRepository _permissionsRepository;

        public ProjectRepository(IApplicationDbContext<ApplicationUser> d, INodeRepository nodeRepository, INotificationReposotory notyfication, IPermissionRepository permissionsRepository)
        {
            db = d;
            _nodeRepository = nodeRepository;
            _notificationRepository = notyfication;
            _permissionsRepository = permissionsRepository;

        }

        // Returns only if you are owner or have permission
        public Project Single(int id)
        {
            var project = db.Projects
                            .Include("AssignedUsers")
                            .Include("Owner")
                            .Include("Nodes")
                            .SingleOrDefault(p => p.Id == id);

            return project;
        }

        // Returns only if you are owner or have permission
        public List<Project> All()
        {
            var projects = from p in db.Projects
                           .Include("AssignedUsers")
                           .Include("Owner")
                           .Include("Nodes")
                           select p;

            return projects.ToList(); ;
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

        // returns true when object deleted
        public bool Delete(int id)
        {
            //using (TransactionScope tr = new TransactionScope())
            //{
            //    try
            //    {
            //        var currentUserId = db.CurrentUserId();
            //        var project = Single(id);

            //        // only owner can delete
            //        if (project?.Owner.Id != currentUserId)
            //        {
            //            return false;
            //        }

            //        // add notification
            //        _notificationRepository.Create(new Notification
            //        {
            //            Author = db.Users.Single(x => x.Id == currentUserId),
            //            Header = "Project deleted",
            //            Priority = 1,
            //            ProjectId = project.Id,
            //            Link = null
            //        });

            //        // remove associations
            //        foreach (var user in project.AssignedUsers)
            //            user.AssignedProjects.Remove(project);
            //        project.AssignedUsers.Clear();

            //        foreach (var node in project.Nodes)
            //            node.Project = null;
            //        project.Nodes.Clear();

            //        var permissions = _permissionsRepository.GetPermissions(projectId: id);
            //        foreach (var permission in permissions)
            //            _permissionsRepository.Remove(permission);

            //        // remove objecy
            //        db.Projects.Remove(project);
            //        db.SaveChanges();

            //        tr.Complete();
            //        return true;
            //    }
            //    catch (Exception e)
            //    {
            return false;
            //    }
            //}
        }


    }


}