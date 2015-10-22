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

    public interface IImageRepository
    {

        Image GetImage(int id);

        List<Image> GetImages();

        void CreateImage(ImageViewModel image);

        bool DeleteImage(int id);

        Task<bool> UploadImage(int id, HttpRequestMessage request);

      

    }


    public class ImageRepository : IImageRepository
    {


        public Image GetImage(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                return db.Images.SingleOrDefault(x=>x.Id == id && x.Project.Owner.Id == currentUserId);
            }
        }


        public List<Image> GetImages()
        {
            using (var db = ApplicationDbContext.Create())
            {

                var currentUserId = HttpContext.Current.User.Identity.GetUserId();
     
                var images = from image in db.Images
                             where image.Project.Owner.Id == currentUserId
                             select image;

                return images.ToList();
            }
        }

        public void CreateImage(ImageViewModel model)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();

                var image = new Image
                {
                    Name = model.Name,
                    Description = model.Description,
                    Project = db.Projects.SingleOrDefault(p=>p.Id==model.ProjectId)
                };

                db.Images.Add(image);
                db.SaveChanges();
            }
        }

        public bool DeleteImage(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var project = db.Projects.Find(id);


                if (project == null)
                {
                    return false;
                }
                if (project.Owner.Id != currentUserId)
                {
                    return false;
                }

                project.Watchers.Clear();
                foreach (var image in project.Images)
                    image.Project = null;
                project.Images.Clear();

                db.Projects.Remove(project);
                db.SaveChanges();

                return true;
            }
        }

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
      



    }


}