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

namespace DesignHubSite.Services
{

    public interface IProjectService
    {

       

        Task<bool> UploadImage(int id, HttpRequestMessage request);

        bool InviteWatcher(int projectId, string userId);

    }


    public class ProjectService : IProjectService
    {


    

        public async Task<bool> UploadImage(int id, HttpRequestMessage request)
        {
            if (!request.Content.IsMimeMultipartContent())
                return false;

            using (var db = ApplicationDbContext.Create())
            {
                // TODO: check if right owner
                var project = db.Projects.Single(x => x.Id == id);

                if (project == null)
                    return false;

                var provider = new MultipartMemoryStreamProvider();
                await request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();

                    //                project.ImageName = filename;
                    //                project.Image = buffer;
                }

                db.SaveChanges();
                return true;

            }
        }

        public bool InviteWatcher(int projectId, string userId)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var project = db.Projects.First(p => p.Id == projectId);
                var user = db.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null && project != null && project.Owner.Id == db.CurrentUserId())
                {
                    project.Watchers.Add(user);
                    user.WatchedProjects.Add(project);

                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



    }


}