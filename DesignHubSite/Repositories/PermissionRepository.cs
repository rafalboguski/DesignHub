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
using System.Data.Entity;

namespace DesignHubSite.Repositories
{

    public interface IPermissionRepository
    {
        Permision GetPermission(string userId, int projectId);
        List<Permision> GetPermissions(string userId = null, int? projectId = null);
    }


    public class PermissionRepository : IPermissionRepository
    {

        private ApplicationDbContext _db = ApplicationDbContext.Create();


        public Permision GetPermission(string userId, int projectId)
        {
            var permission = _db.Permisions
                    .Include(x => x.Project)
                    .Include(x => x.User)
                    .SingleOrDefault(x => x.User.Id == userId && x.Project.Id == projectId);

            return permission;
        }


        public List<Permision> GetPermissions(string userId = null, int? projectId = null)
        {
            var permissions = _db.Permisions.Include(x => x.Project).Include(x => x.User);

            if (userId == null && projectId == null)
                return null;

            if (userId != null)
                return permissions.Where(x => x.User.Id == userId).ToList();

            if (projectId != null)
                return permissions.Where(x => x.Project.Id == projectId).ToList();

            return null;
        }

    }

}