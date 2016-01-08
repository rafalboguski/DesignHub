﻿using System;
using System.Collections.Generic;
using System.Linq;
using DesignHubSite.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using DesignHubSite.ExtensionMethods;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace DesignHubSite.Repositories
{

    public interface INodeRepository
    {

        Node Single(int? id);

        List<Node> All(int? projectId = null);

        Node Create(NodeDTO nodeDto);

        int Update(int nodeId, Node data);

        bool Delete(int id);

        Task<bool> UploadImage(int id, HttpRequestMessage request);


        List<ApplicationUser> Like(int nodeID);
        List<ApplicationUser> Dislike(int nodeID);

        void Accept(int nodeID);
        void Reject(int nodeID);


    }


    public class NodeRepository : INodeRepository
    {
        private IApplicationDbContext<ApplicationUser> _db;
        private INotificationReposotory _notyfication;
        private IPermissionRepository _permissionsRepository;

        public NodeRepository(IApplicationDbContext<ApplicationUser> db, INotificationReposotory notyfication, IPermissionRepository permissionsRepository)
        {
            _db = db;
            _notyfication = notyfication;
            _permissionsRepository = permissionsRepository;
        }


        public Node Single(int? id)
        {
            var currentUserId = _db.CurrentUserId();

            var node = _db.Nodes
                            .Include(x => x.Project)
                            .Include("Project.Owner")
                            .Include(x => x.Childrens)
                            .Include(x => x.ImageMarkers)
                            .Include(x => x.whoRejected)
                            .Include(x => x.whoAccepted)
                            .Include(x => x.Likes)
                            .Include(x => x.Dislikes)
                            .SingleOrDefault(p => (p.Id == id));
            if (node == null)
                return node;

            var permision = _permissionsRepository.GetPermission(currentUserId, node.Project.Id);
            var loggedUser = _db.Users.SingleOrDefault(x => x.Id == currentUserId);

            if (node.Project.Owner.Id != loggedUser.Id)
                if (permision == null || permision.Readonly == false)
                {
                    return null;
                }

            return node;
        }


        public List<Node> All(int? projectId = null)
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            List<Node> nodes;

            if (projectId == null)
            {
                nodes = (from p in _db.Nodes
                           .Include(x => x.Project)
                            .Include(x => x.Childrens)
                            .Include("Project.Owner")
                            .Include(x => x.ImageMarkers)
                         select p).ToList();
            }
            else
            {
                nodes = (from p in _db.Nodes
                             .Include(x => x.Project)
                            .Include(x => x.Childrens)
                            .Include("Project.Owner")
                            .Include(x => x.ImageMarkers)
                         where p.Project.Id == projectId
                         select p).ToList();
            }

            if (nodes != null && nodes.Count > 0)
            {
                var permision = _permissionsRepository.GetPermission(currentUserId, nodes.First().Project.Id);
                var loggedUser = _db.Users.SingleOrDefault(x => x.Id == currentUserId);

                if (nodes.First().Project.Owner.Id != loggedUser.Id)
                    if (permision == null || permision.Readonly == false)
                    {
                        return null;
                    }
            }

            return nodes.ToList();
        }

        /*
            if previous nod is null then node is root
        */
        public Node Create(NodeDTO nodeDto)
        {
            // int? parentId

            var currentUserId = _db.CurrentUserId();
            var currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);

            if (nodeDto.ProjectId == null)
                return null;
            var project = _db.Projects.Include("Owner").SingleOrDefault(p => p.Id == nodeDto.ProjectId);

            if (project.Owner.Id != currentUser.Id)
                return null;

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
            if (nodeDto.ParentsId.Count() == 0)
            {
                node.Root = true;
                node.Accepted = true;
            }
            else
            {
                foreach (var id in nodeDto.ParentsId)
                {
                    var parent = _db.Nodes.Single(x => x.Id == id);
                    node.Parents.Add(parent);
                    parent.Childrens.Add(node);

                }

            }


            _db.Nodes.Add(node);
            _db.SaveChanges();

            if (!node.Root)
                _notyfication.Create(new Notification
                {
                    Author = currentUser,
                    Header = "New node",
                    Priority = 4,
                    ProjectId = project.Id,
                    Link = "/project/" + project.Id + "/graph/" + node.Id
                });

            return node;
        }

        public int Update(int nodeId, Node data)
        {
            var currentUserId = _db.CurrentUserId();
            var currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);

            var node = _db.Nodes.Include("Project.Owner").SingleOrDefault(n => n.Id == nodeId);

            if (node.Project.Owner.Id != currentUser.Id)
                return node.Id;

            node.positionX = data.positionX;
            node.positionY = data.positionY;


            _db.SaveChanges();
            return node.Id;
        }


        public bool Delete(int id)
        {
            return true;
        }

        public async Task<bool> UploadImage(int id, HttpRequestMessage request)
        {
            if (!request.Content.IsMimeMultipartContent())
                return false;

            // TODO: check if right owner
            var node = _db.Nodes.Include("Project.Owner").Single(x => x.Id == id);

            if (node == null)
                return false;

            var currentUserId = _db.CurrentUserId();
            if (node.Project.Owner.Id != currentUserId)
                return false;

            var provider = new MultipartMemoryStreamProvider();
            await request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();

                //                project.ImageName = filename;
                node.Image = buffer;
                node.Thumbnail = MakeThumbnail(buffer, 200, 200);
            }

            _db.SaveChanges();
            return true;
        }

        private static byte[] MakeThumbnail(byte[] myImage, int thumbWidth, int thumbHeight)
        {
            Bitmap source = new Bitmap(Image.FromStream(new MemoryStream(myImage)));
            int minSize = source.Width;
            if (source.Height < minSize)
                minSize = source.Height;

            Rectangle rect = new Rectangle(0, 0, minSize, minSize);
            Bitmap cropped = source.Clone(rect, source.PixelFormat);

            using (MemoryStream ms = new MemoryStream())
            using (Image thumbnail = cropped.GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
            {

                thumbnail.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }


        public List<ApplicationUser> Like(int nodeID)
        {
            var node = _db.Nodes.Include(x => x.Project).Single(x => x.Id == nodeID);

            var userId = _db.CurrentUserId();
            var user = _db.Users.Single(x => x.Id == userId);

            if (node.Likes.Contains(user))
            {
                node.Likes.Remove(user);
            }
            else
            {
                node.Dislikes.Remove(user);
                node.Likes.Add(user);
            }

            _db.SaveChanges();

            _notyfication.Create(new Notification
            {
                Author = user,
                Header = "User " + user.UserName + " likes node(image): " + node.Id,
                Priority = 2,
                ProjectId = node.Project.Id,
                Link = "/project/" + node.Project.Id + "/graph/" + node.Id
            });

            return node.Likes.ToList();

        }

        public List<ApplicationUser> Dislike(int nodeID)
        {
            var node = _db.Nodes.Single(x => x.Id == nodeID);

            var userId = _db.CurrentUserId();
            var user = _db.Users.Single(x => x.Id == userId);

            if (node.Dislikes.Contains(user))
            {
                node.Dislikes.Remove(user);
            }
            else
            {
                node.Likes.Remove(user);
                node.Dislikes.Add(user);
            }

            _db.SaveChanges();

            _notyfication.Create(new Notification
            {
                Author = user,
                Header = "User " + user.UserName + " dislikes node(image): " + node.Id,
                Priority = 2,
                ProjectId = node.Project.Id,
                Link = "/project/" + node.Project.Id + "/graph/" + node.Id
            });
            return node.Dislikes.ToList();
        }

        public void Accept(int nodeID)
        {
            var loggedUserId = _db.CurrentUserId();
            var loggedUser = _db.Users.Single(x => x.Id == loggedUserId);

            var node = _db.Nodes.Include(x => x.Project).Single(x => x.Id == nodeID);
            node.Accepted = !node.Accepted;
            node.whoAccepted = loggedUser;
            _db.SaveChanges();

            _notyfication.Create(new Notification
            {
                Author = loggedUser,
                Header = "User " + loggedUser.UserName + " accepted node(image): " + node.Id,
                Priority = 5,
                ProjectId = node.Project.Id,
                Link = "/project/" + node.Project.Id + "/graph/" + node.Id
            });
        }

        public void Reject(int nodeID)
        {
            var loggedUserId = _db.CurrentUserId();
            var loggedUser = _db.Users.Single(x => x.Id == loggedUserId);

            var node = _db.Nodes.Include(x => x.Project)
                               .Single(x => x.Id == nodeID);
            node.Rejected = !node.Rejected;
            node.whoRejected = loggedUser;
            _db.SaveChanges();

            _notyfication.Create(new Notification
            {
                Author = loggedUser,
                Header = "User " + loggedUser.UserName + " rejected node(image): " + node.Id,
                Priority = 5,
                ProjectId = node.Project.Id,
                Link = "/project/" + node.Project.Id + "/graph/" + node.Id
            });

        }
    }


}