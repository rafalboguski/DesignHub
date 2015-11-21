using System;
using System.Collections.Generic;
using System.Linq;
using DesignHubSite.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using DesignHubSite.ExtensionMethods;

namespace DesignHubSite.Services
{

    public interface INodeRepository
    {

        Node Single(int id);

        List<Node> All(int? projectId = null);

        int Create(Node node, int projectId, int? previousNodeId);

        bool Delete(int id);


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
                                select p;
                    return nodes.ToList();
                }
                else
                {
                    var nodes = from p in db.Nodes
                                .Include("Project")
                                .Include("Childrens")
                                where p.Project.Id == projectId
                                select p;
                    return nodes.ToList();
                }

            }
        }

        /*
            if previous nod is null then node is root
        */
        public int Create(Node node, int projectId, int? previousNodeId)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                var project = db.Projects.SingleOrDefault(p => p.Id == projectId);
                project.Nodes.Add(node);

                // is root
                if (previousNodeId == null)
                {
                    node.Root = true;
                }
                else
                {
                    var previousNode = db.Nodes.SingleOrDefault(n => n.Id == previousNodeId);
                    previousNode.Childrens.Add(node);
                    node.Parent = previousNode;
                }


                db.Nodes.Add(node);
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


    }


}