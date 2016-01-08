using System;
using System.Collections.Generic;
using System.Linq;
using DesignHubSite.Models;
using DesignHubSite.ExtensionMethods;
using DesignHubSite.Repositories;

namespace DesignHubSite.Services
{

    public interface IProjectService
    {

        Project GetLoggedUserVisibleProject(int projectId);
        List<Project> GetLoggedUserVisibleProjects();

        void setHead(int id);
        void AcceptProject(int projectId, out List<string> errors);
        void RejectProject(int projectId, out List<string> errors);
        List<ProjectNote> GetNotes(int projectId);
        ProjectNote AddNote(int projectId, string text);
        void RemoveNote(int id);

        void Test();

    }


    public class ProjectService : IProjectService
    {

        private IApplicationDbContext<ApplicationUser> _db;

        private INotificationReposotory _notyfication;
        private IRepository<Project> _projectsRepository;

        private IUsersService _usersService;
        private IPermissionRepository _permissionsRepository;

        public ProjectService(
            IApplicationDbContext<ApplicationUser> db,
            INotificationReposotory notyfication, IRepository<Project> projectsRepository,
            IUsersService usersService,
            IPermissionRepository permissionsRepository)
        {
            _db = db;
            _notyfication = notyfication;
            _projectsRepository = projectsRepository;
            _permissionsRepository = permissionsRepository;

            _usersService = usersService;
        }


        // only owner or user with read permussion can see project
        public Project GetLoggedUserVisibleProject(int projectId)
        {
            var loggedUser = _usersService.LoggedUser();
            var project = _projectsRepository.Single(projectId);
            var permision = _permissionsRepository.GetPermission(loggedUser.Id, project.Id);

            if (project.Owner.Id != loggedUser.Id)
                if (permision == null || permision.Readonly == false)
                {
                    return null;
                }
            return project;
        }

        public List<Project> GetLoggedUserVisibleProjects()
        {
            var loggedUser = _usersService.LoggedUser();
            var projects = loggedUser.AssignedProjects.ToList();
            projects.AddRange(loggedUser.OwnedProjects.ToList());

            var list = new List<Project>();

            foreach (var project in projects)
            {
                if (project.Owner.Id == loggedUser.Id)
                {
                    list.Add(project);
                    continue;
                }
                var permission = _permissionsRepository.GetPermission(loggedUser.Id, project.Id);
                if (permission != null && permission.Readonly == true)
                {
                    list.Add(project);
                }

            }
            return list;
        }


        //

        public void AcceptProject(int projectId, out List<string> errors)
        {
            errors = new List<string>();

            var loggedUser = _usersService.LoggedUser();
            var project = _projectsRepository.Single(projectId);

            var permision = _permissionsRepository.GetPermission(loggedUser.Id, project.Id);
            if (project.Owner.Id != loggedUser.Id)
                if (permision == null || permision.AcceptWholeProject == false)
                {
                    errors.Add("No permission");
                    return;
                }

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
                Header = "Project status changed",
                Priority = 10,
                CreateDate = DateTime.Now,
                ProjectId = project.Id,
                Link = "/project/" + project.Id
            });

            _db.SaveChanges();
        }

        public void RejectProject(int projectId, out List<string> errors)
        {
            errors = new List<string>();

            var loggedUser = _usersService.LoggedUser();
            var project = _projectsRepository.Single(projectId);
            var permision = _permissionsRepository.GetPermission(loggedUser.Id, project.Id);
            if (project.Owner.Id != loggedUser.Id)
                if (permision == null || permision.AcceptWholeProject == false)
                {
                    errors.Add("No permission");
                    return;
                }

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
                Header = "Project status changed",
                Priority = 10,
                CreateDate = DateTime.Now,
                ProjectId = project.Id,
                Link = "/project/" + project.Id
            });
            _db.SaveChanges();
        }

        //

        public ProjectNote AddNote(int projectId, string text)
        {
            var user = _usersService.LoggedUser();
            var project = _projectsRepository.Single(projectId);

            if (project.Owner.Id != user.Id)
                return null;

            if (text == null)
                return null;

            var note = new ProjectNote
            {
                ProjectId = projectId,
                content = text
            };
            _db.ProjectsNotes.Add(note);
            _db.SaveChanges();
            return note;
        }

        public void setHead(int id)
        {
            var head = _db.Nodes.Include("Project.Owner").SingleOrDefault(n => n.Id == id);
            if (head.Project.Owner.Id != _usersService.LoggedUserId())
                return;

            head.Head = true;

            var projectId = head.Project.Id;
            head.Project.nodeHeadId = id;

            _db.Nodes.Include("Project")
                    .Where(x => x.Project.Id == projectId && x.Id != head.Id).ToList()
                    .ForEach(x => x.Head = false);


            _db.SaveChanges();

            var userId = _usersService.LoggedUserId();
            _notyfication.Create(new Notification
            {
                Author = _db.Users.Single(x => x.Id == userId),
                Header = "New node: head",
                Priority = 5,
                CreateDate = DateTime.Now,
                ProjectId = head.Project.Id,
                Link = "/project/" + head.Project.Id + "/graph/" + head.Id
            });
        }

        void IProjectService.RemoveNote(int id)
        {

            var note = _db.ProjectsNotes.Single(x => x.Id == id);
            var user = _usersService.LoggedUser();
            var project = _projectsRepository.Single(note.ProjectId);

            if (project.Owner.Id != user.Id)
                return;

            _db.ProjectsNotes.Remove(note);
            _db.SaveChanges();

        }

        public List<ProjectNote> GetNotes(int projectId)
        {
            var user = _usersService.LoggedUser();
            var project = _projectsRepository.Single(projectId);

            if (project.Owner.Id != user.Id)
                return null;

            var notes = _db.ProjectsNotes.Where(x => x.ProjectId == projectId).OrderByDescending(x => x.Id).ToList();
            return notes;
        }

        public void Test()
        {
            var user = _usersService.LoggedUser();

            var perm = _permissionsRepository.GetPermissions(user.Id);




            var end = 3;






        }
    }


}