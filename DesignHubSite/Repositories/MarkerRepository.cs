using System;
using System.Collections.Generic;
using System.Linq;
using DesignHubSite.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using DesignHubSite.ExtensionMethods;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Transactions;

namespace DesignHubSite.Repositories
{

    public interface IMarkerRepository
    {

        Marker Single(int id);

        List<Marker> All(int nodeId);

        Marker Create(MarkerDto model, out string error);

        void ReplyToOpinion(int MarkerOpinionId, string text);

        bool Delete(int id);

    }


    public class MarkerRepository : IMarkerRepository
    {

        private IApplicationDbContext<ApplicationUser> _db;
        private INotificationReposotory _notyfication;

        public MarkerRepository(IApplicationDbContext<ApplicationUser>  db, INotificationReposotory notyfication)
        {
            _db = db;
            _notyfication = notyfication;
        }

        public Marker Single(int id)
        {
            var marker = _db.Markers
                .Include(x => x.Opinions)
                .Include(x => x.Opinions.Select(o => o.Author))
                .Include(x => x.Opinions.Select(o => o.Replies))
                .SingleOrDefault(m => (m.Id == id));

            return marker;
        }


        public List<Marker> All(int nodeId)
        {
            var markers = _db.Markers
                .Include(x => x.Opinions)
                .Include(x => x.Opinions.Select(o => o.Author))
                .Include(x => x.Opinions.Select(o => o.Replies))
                .Where(m => m.Node.Id == nodeId);
            return markers.ToList();
        }


        public Marker Create(MarkerDto dto, out string error)
        {
            error = null;
            using (var tr = new TransactionScope())
            {
                try
                {
                    var userId = _db.CurrentUserId();
                    var user = _db.Users.Single(x => x.Id == userId);

                    var opinion = new MarkerOpinion
                    {
                        Author = user,
                        Opinion = dto.Text,
                        Timestamp = DateTime.Now
                    };

                    Marker marker;

                    if (dto.Id == null) // new marker
                    {
                        marker = new Marker
                        {
                            Height = dto.Height,
                            Width = dto.Width,
                            X = dto.X,
                            Y = dto.Y,
                            Number = _db.Markers.Include(x => x.Node)
                                               .Where(x => x.Node.Id == dto.NodeId)
                                               .Count() + 1,
                            Node = _db.Nodes.Include(n => n.Project).SingleOrDefault(n => n.Id == dto.NodeId)
                        };
                        _db.Markers.Add(marker);
                    }
                    else  // adding opinion to existing marker
                    {
                        marker = _db.Markers.Include("Node").Include("Node.Project").Single(x => x.Id == dto.Id);
                    }

                    opinion.Marker = marker;
                    marker.Opinions.Add(opinion);
                    _db.MarkersOpinions.Add(opinion);
                    _db.SaveChanges();

                    _notyfication.Create(new Notification
                    {
                        Author = user,
                        Header = "Node: new opinion",
                        Priority = 4,
                        ProjectId = marker.Node.Project.Id,
                        Link = "/project/" + marker.Node.Project.Id + "/markers/" + marker.Node.Id
                    });

                    tr.Complete();
                    return marker;
                }
                catch (Exception e)
                {
              
                    error = "DB_ERROR :" + e.Message;
                    return null;
                }
            }
        }


        public bool Delete(int id)
        {
            var marker = _db.Markers.SingleOrDefault(m => m.Id == id);
            _db.Markers.Remove(marker);
            return _db.SaveChanges() == 0;
        }


        public void ReplyToOpinion(int MarkerOpinionId, string text)
        {
            var loggedUserId = _db.CurrentUserId();
            var loggedUser = _db.Users.Single(x => x.Id == loggedUserId);

            var opinion = _db.MarkersOpinions.Include("Marker.Node.Project")
                                             .Single(x => x.Id == MarkerOpinionId);

            var reply = new MarkerOpinionReply
            {
                Opinion = opinion,
                Text = text,
                Author = loggedUser,
                Timestamp = DateTime.Now
            };

            opinion.Replies.Add(reply);
            _db.MarkersOpinionsReplies.Add(reply);
            _db.SaveChanges();

            _notyfication.Create(new Notification
            {
                Author = loggedUser,
                Header = "Reply to node opinion",          
                Priority = 4,
                ProjectId = reply.Opinion.Marker.Node.Project.Id,
                Link = "/project/" + reply.Opinion.Marker.Node.Project.Id + "/markers/" + reply.Opinion.Marker.Node.Id
            });
        }

    }


}