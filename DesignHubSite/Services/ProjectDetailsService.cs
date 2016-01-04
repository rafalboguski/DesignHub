using DesignHubSite.Models;
using DesignHubSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DesignHubSite.ExtensionMethods;

namespace DesignHubSite.Services
{

    public interface IProjectDetailsService
    {
        bool InviteUserToProject(PermisionDto model);

    }



    public class ProjectDetailsService : IProjectDetailsService
    {
        private INotificationReposotory _notyfication;


        public ProjectDetailsService(INotificationReposotory notyfication)
        {
            _notyfication = notyfication;
        }


        public bool InviteUserToProject(PermisionDto model)
        {
            using (var db = ApplicationDbContext.Create())
            {

                // todo: create user repo

                var user = db.Users.SingleOrDefault(x => x.Id == model.UserId);
                var project = db.Projects.SingleOrDefault(x => x.Id == model.ProjectId);

                if (user == null || project == null)
                    return false;

                var permision = new Permision
                {
                    User = user,
                    Project = project,
                    ProjectRole = model.ProjectRole,
                    AcceptNodes = model.AcceptNodes,
                    AcceptWholeProject = model.AcceptWholeProject,
                    AddMarkers = model.AddMarkers,
                    LikeOrDislikeChanges = model.LikeOrDislikeChanges,
                    Message = model.Message,
                    Readonly = model.Readonly,
                    Timestamp = DateTime.Now
                };

                var tmp = db.Permisions
                    .Include(x => x.User)
                    .Include(x => x.Project)
                    .Where(x => x.User.Id == model.UserId && x.Project.Id == model.ProjectId)
                    .ToList();
                if (tmp.Count() == 0)
                {
                    db.Permisions.Add(permision);
                    project.AssignedUsers.Add(user);
                    user.AssignedProjects.Add(project);

                }
                else
                {
                    int id = tmp.First().Id;
                    Permision per = db.Permisions.SingleOrDefault(x => x.Id ==id);

                    per.ProjectRole = model.ProjectRole;
                    per.AcceptNodes = model.AcceptNodes;
                    per.AcceptWholeProject = model.AcceptWholeProject;
                    per.AddMarkers = model.AddMarkers;
                    per.LikeOrDislikeChanges = model.LikeOrDislikeChanges;
                    per.Message = model.Message;
                    per.Readonly = model.Readonly;
                    per.Timestamp = DateTime.Now;

                }

                //

                db.SaveChanges();

                var userId = db.CurrentUserId();
                _notyfication.Create(new Notification
                {
                    Author = db.Users.Single(x => x.Id == userId),
                    Header = "User "+ user.UserName + " has his rights to this project changed",
                    CreateDate = DateTime.Now,
                    Priority = 1,
                    ProjectId = model.ProjectId.Value,
                });

                return true;
            }


        }
    }
}