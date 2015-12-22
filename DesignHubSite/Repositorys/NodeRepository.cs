using System;
using System.Collections.Generic;
using System.Linq;
using DesignHubSite.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using DesignHubSite.ExtensionMethods;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesignHubSite.Repositories
{

    public interface INodeRepository
    {

        Node Single(int id);

        List<Node> All(int? projectId = null);

        Node Create(NodeDTO nodeDto);

        int Update(int nodeId, Node data);

        bool Delete(int id);

        Task<bool> UploadImage(int id, HttpRequestMessage request);
    }


    public class NodeRepository : INodeRepository
    {


        public Node Single(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var currentUserId = db.CurrentUserId();

                var node = db.Nodes
                                .Include("Project")
                                .Include("Childrens")
                                .Include("ImageMarkers")
                                .SingleOrDefault(p => (p.Id == id));

                return node;
            }
        }


        public List<Node> All(int? projectId = null)
        {
            using (var db = ApplicationDbContext.Create())
            {

                var currentUserId = HttpContext.Current.User.Identity.GetUserId();

                if (projectId == null)
                {
                    var nodes = from p in db.Nodes
                               .Include("Project")
                               .Include("Childrens")
                               .Include("ImageMarkers")
                                select p;
                    return nodes.ToList();
                }
                else
                {
                    var nodes = from p in db.Nodes
                                .Include("Project")
                                .Include("Childrens")
                                .Include("ImageMarkers")
                                where p.Project.Id == projectId
                                select p;
                    return nodes.ToList();
                }

            }
        }

        /*
            if previous nod is null then node is root
        */
        public Node Create(NodeDTO nodeDto)
        {
            using (var db = ApplicationDbContext.Create())
            {

                // int? parentId

                var currentUserId = db.CurrentUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                if (nodeDto.ProjectId == null)
                    return null;
                var project = db.Projects.SingleOrDefault(p => p.Id == nodeDto.ProjectId);

                var node = new Node
                {
                    ChangeInfo = nodeDto.ChangeInfo,
                    Image = nodeDto.Image,
                    positionX = nodeDto.positionX,
                    positionY = nodeDto.positionY,
                    Project = project,
                    Timestamp = nodeDto.date
                    
                };

                project.Nodes.Add(node);
                node.Project = project;

                // is root
                if (nodeDto.ParentId == null)
                {
                    node.Root = true;
                }
                else
                {
                    var parent = db.Nodes.SingleOrDefault(n => n.Id == nodeDto.ParentId);
                    parent.Childrens.Add(node);
                    node.Parent = parent;
                }


                db.Nodes.Add(node);
                db.SaveChanges();
                return node;
            }
        }

        public int Update(int nodeId, Node data)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);


                var node = db.Nodes.SingleOrDefault(n => n.Id == nodeId);

                node.positionX = data.positionX;
                node.positionY = data.positionY;


                db.SaveChanges();
                return node.Id;
            }
        }


        public bool Delete(int id)
        {
            //using (var db = ApplicationDbContext.Create())
            //{
            //    var currentUserId = db.CurrentUserId();
            //    var node = db.Nodes.Find(id);


            //    if (node == null)
            //    {
            //        return false;
            //    }
            //    if (node.Project.Owner.Id != currentUserId)
            //    {
            //        return false;
            //    }

            //    foreach(var child in node.Childs)
            //    {
            //        Delete(child.Id);
            //    }

            //    node.Childs.Clear();
            //    node.Father.Childs.Remove(node);


            //    node.Project.Nodes.Remove(node);


            //    node.Project = null;
            //    db.Nodes.Remove(node);

            //    db.SaveChanges();

            //   
            return true;
            //}
        }

        public async Task<bool> UploadImage(int id, HttpRequestMessage request)
        {
            if (!request.Content.IsMimeMultipartContent())
                return false;

            using (var db = ApplicationDbContext.Create())
            {
                // TODO: check if right owner
                var node = db.Nodes.Single(x => x.Id == id);

                if (node == null)
                    return false;

                var provider = new MultipartMemoryStreamProvider();
                await request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();

                    //                project.ImageName = filename;
                    node.Image = buffer;
                }

                db.SaveChanges();
                return true;

            }
        }
    }


}