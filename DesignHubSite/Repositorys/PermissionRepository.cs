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

        Permision Permission(string userId, int projectId);

        List<Permision> Permissions(string userId = null, int? projectId = null);

    }


    public class PermissionRepository : IPermissionRepository
    {


        public Permision Permission(string userId, int projectId)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var obj = db.Permisions
                        .Include(x => x.Project)
                        .Include(x => x.User)
                        .SingleOrDefault(x => x.User.Id == userId && x.Project.Id == projectId);

                return obj;
            }
        }


        public List<Permision> Permissions(string userId = null, int? projectId = null)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var list = db.Permisions.Include(x => x.Project).Include(x => x.User);

                if (userId == null && projectId == null)
                    return null;

                if (userId != null)
                    return list.Where(x => x.User.Id == userId).ToList();

                if (projectId != null)
                    return list.Where(x => x.Project.Id == projectId).ToList();

                return null;
            }
        }



    }


}