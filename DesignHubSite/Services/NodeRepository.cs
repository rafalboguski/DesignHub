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

        List<Node> All();

        void Create(Node node, int projectId, int? previousNodeId);

        bool Delete(int id);


    }


    public class NodeRepository : INodeRepository
    {


        public Node Single(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();

                var node = db.Nodes
                                .Include("Project")
                                .Include("Childs")
                                .SingleOrDefault(p => (p.Id == id));

                return node;
            }
        }


        public List<Node> All()
        {
            using (var db = ApplicationDbContext.Create())
            {

                var currentUserId = HttpContext.Current.User.Identity.GetUserId();
                var nodes = from p in db.Nodes
                               .Include("Project")
                               .Include("Childs")
                                select p;



                return nodes.ToList();
            }
        }

        /*
            if previous nod is null then node is root
        */
        public void Create(Node node, int projectId, int?  previousNodeId)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                var project = db.Projects.SingleOrDefault(p => p.Id == projectId);

                if(previousNodeId == null)
                {
                    //if(project.Root != null)
                    //{
                    //    throw new Exception("Project has already root node");
                    //}

                    node.Father = null;
                    project.Root = node;
                }
                else
                {
                    var previousNode = db.Nodes.SingleOrDefault(n=>n.Id == previousNodeId);
                    node.Father = previousNode;
                }

                project.Nodes.Add(node);

                db.Nodes.Add(node);
                db.SaveChanges();
            }
        }

        public bool Delete(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var currentUserId = db.CurrentUserId();
                var node = db.Nodes.Find(id);


                if (node == null)
                {
                    return false;
                }
                if (node.Project.Owner.Id != currentUserId)
                {
                    return false;
                }

                foreach(var child in node.Childs)
                {
                    Delete(child.Id);
                }

                node.Childs.Clear();
                node.Father.Childs.Remove(node);


                node.Project.Nodes.Remove(node);
                if(node.Project.Head == node)
                {
                    node.Project.Head = null;
                }
                if (node.Project.Root == node)
                {
                    node.Project.Root = null;
                }


                node.Project = null;
                db.Nodes.Remove(node);

                db.SaveChanges();

                return true;
            }
        }

   
    }


}