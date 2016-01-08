using DesignHubSite.Models;
using DesignHubSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DesignHubSite.ExtensionMethods;
using System.Transactions;

namespace DesignHubSite.Services
{

    public interface IProjectDetailsService
    {
        bool InviteUserToProject(PermisionDto model);

    }



    public class ProjectDetailsService : IProjectDetailsService
    {

        private IApplicationDbContext<ApplicationUser> db ;

        private INotificationReposotory _notyfication;


        public ProjectDetailsService(IApplicationDbContext<ApplicationUser> d, INotificationReposotory notyfication)
        {
            db = d;
            _notyfication = notyfication;
        }


        public bool InviteUserToProject(PermisionDto model)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var user = db.Users.SingleOrDefault(x => x.Id == model.UserId);
                    var project = db.Projects.Include("Owner").SingleOrDefault(x => x.Id == model.ProjectId);

                    if(db.CurrentUserId() != project.Owner.Id)
                    {
                        return false;
                    }

                    if (user == null || project == null)
                        return false;

                    var tmp = db.Permisions
                        .Include(x => x.User)
                        .Include(x => x.Project)
                        .SingleOrDefault(x => x.User.Id == model.UserId && x.Project.Id == model.ProjectId);

                    if (tmp == null)
                    {
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

                        db.Permisions.Add(permision);
                        project.AssignedUsers.Add(user);
                        user.AssignedProjects.Add(project);

                    }
                    else
                    {
                        int id = tmp.Id;
                        Permision per = db.Permisions.SingleOrDefault(x => x.Id == id);

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
                        Header = "User " + user.UserName + " has his rights to this project changed",
                        CreateDate = DateTime.Now,
                        Priority = 1,
                        ProjectId = model.ProjectId.Value,
                    });

                    transaction.Complete();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }


        }
    }
}