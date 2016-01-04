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
using DesignHubSite.Repositories;

namespace DesignHubSite.Services
{

    public interface IProjectService
    {

        void setHead(int id);

        void AcceptProject(int projectId, out List<string> errors);
        void RejectProject(int projectId, out List<string> errors);

    }


    public class ProjectService : IProjectService
    {

        private ApplicationDbContext _db = ApplicationDbContext.Create();
        private INotificationReposotory _notyfication;
               
        public ProjectService(INotificationReposotory notyfication)
        {
            _notyfication = notyfication;
        }

        public void AcceptProject(int projectId, out List<string> errors)
        {
            errors = new List<string>();

            var loggedUserId = _db.CurrentUserId();
            var loggedUser = _db.Users.Single(x => x.Id == loggedUserId);

            var project = _db.Projects.Single(x => x.Id == projectId);

            if (project.Rejected)
            {
                errors.Add("ERROR: You can't accept rejected project");
                return;
            }

            if (project.Accepted == false)
            {
                project.Accepted = true;
                project.WhoAccepted = loggedUser;
                project.EndDate = DateTime.Now;
            }
            else
            {
                project.Accepted = false;
                project.WhoAccepted = null;
            }

            _notyfication.Create(new Notification
            {
                Author = project.WhoAccepted,
                Header = "Project Accepted",
                Priority = 10,
                CreateDate = DateTime.Now,
                ProjectId = project.Id,
            });

            _db.SaveChanges();
        }

        public void RejectProject(int projectId, out List<string> errors)
        {
            errors = new List<string>();

            var loggedUserId = _db.CurrentUserId();
            var loggedUser = _db.Users.Single(x => x.Id == loggedUserId);

            var project = _db.Projects.Single(x => x.Id == projectId);

            if (project.Accepted)
            {
                errors.Add("ERROR: You can't reject accepted project");
                return;
            }

            if (project.Rejected == false)
            {
                project.Rejected = true;
                project.WhoRejected = loggedUser;
                project.EndDate = DateTime.Now;
            }
            else
            {
                project.Rejected = false;
                project.WhoRejected = null;
                project.EndDate = null;
            }

            _notyfication.Create(new Notification
            {
                Author = project.WhoAccepted,
                Header = "Project Rejected",
                Priority = 10,
                CreateDate = DateTime.Now,
                ProjectId = project.Id,
                Link = null
            });
            _db.SaveChanges();
        }

        public void setHead(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var head = db.Nodes.Include("Project").SingleOrDefault(n => n.Id == id);
                head.Head = true;

                var projectId = head.Project.Id;
                head.Project.nodeHeadId = id;

                db.Nodes.Include("Project")
                    .Where(x => x.Project.Id == projectId && x.Id != head.Id).ToList()
                    .ForEach(x => x.Head = false);


                db.SaveChanges();

                var userId = _db.CurrentUserId();
                _notyfication.Create(new Notification
                {
                    Author = db.Users.Single(x => x.Id == userId),
                    Header = "Project has new default image",
                    Content = null,
                    Priority = 5,
                    CreateDate = DateTime.Now,
                    ProjectId = head.Project.Id,
                    Link = null
                });
            }
        }


    }


}