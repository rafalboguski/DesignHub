using DesignHubSite.Models;
using DesignHubSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DesignHubSite.ExtensionMethods;
using System.Data.Entity;

namespace DesignHubSite.Services
{

    public interface IPermissionsService
    {
    }


    public class PermissionsService : IPermissionsService
    {

        //private ApplicationDbContext _db = ApplicationDbContext.Create();
        //private IPermissionRepository _permissionsRepository;
        //private IRepository<Project> _projectRepository;

        //public PermissionsService(IPermissionRepository permissionsRepository, IRepository<Project> projectRepository)
        //{
        //    _permissionsRepository = permissionsRepository;
        //    _projectRepository = projectRepository;
        //}


        //public bool CanUserGetProject(string userId, int projectId)
        //{
        //    var user = _db.Users.Single(x => x.Id == userId);
        //    var project = _projectRepository.Single(projectId);

        //    // if owns project
        //    if (project.Owner.Id == user.Id)
        //    {
        //        return true;
        //    }
        //    // if is assigned to it and have proper permission
        //    var permission = _permissionsRepository.GetPermission(user.Id, project.Id);
        //    if (permission.Readonly)
        //    {
        //        return true;
        //    }
            
        //    return false;
        //}

    }

}