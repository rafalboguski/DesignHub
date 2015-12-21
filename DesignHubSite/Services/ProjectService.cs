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



        //Task<bool> UploadImage(int id, HttpRequestMessage request);
      
        void setHead(int id);
    }


    public class ProjectService : IProjectService
    {




        //public async Task<bool> UploadImage(int id, HttpRequestMessage request)
        //{
        //    if (!request.Content.IsMimeMultipartContent())
        //        return false;

        //    using (var db = ApplicationDbContext.Create())
        //    {
        //        // TODO: check if right owner
        //        var node = db.Nodes.Single(x => x.Id == id);

        //        if (node == null)
        //            return false;

        //        var provider = new MultipartMemoryStreamProvider();
        //        await request.Content.ReadAsMultipartAsync(provider);
        //        foreach (var file in provider.Contents)
        //        {
        //            var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
        //            var buffer = await file.ReadAsByteArrayAsync();

        //            //                project.ImageName = filename;
        //                            node.Image = buffer;
        //        }

        //        db.SaveChanges();
        //        return true;

        //    }
        //}
     

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

            }

        }
    }


}