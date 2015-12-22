using DesignHubSite.Models;
using DesignHubSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;                 

namespace DesignHubSite.Services
{

    public interface IProjectDetailsService
    {
        bool InviteUserToProject(PermisionDto model);

    }



    public class ProjectDetailsService : IProjectDetailsService
    {
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

                db.Permisions.Add(permision);
                project.AssignedUsers.Add(user);
                user.AssignedProjects.Add(project);
                //

                db.SaveChanges();
                return true;
            }


        }
    }
}