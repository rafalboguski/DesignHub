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

namespace DesignHubSite.Repositories
{

    public interface IMarkerRepository
    {

        Marker Single(int id);

        List<Marker> All(int nodeId);

        Marker Create(MarkerDto model);

        void ReplyToOpinion(int MarkerOpinionId, string text);

        bool Delete(int id);

    }


    public class MarkerRepository : IMarkerRepository
    {

        public Marker Single(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var marker = db.Markers
                    .Include(x => x.Opinions)
                    .Include(x => x.Opinions.Select(o => o.Author))
                    .Include(x => x.Opinions.Select(o => o.Replies))
                    .SingleOrDefault(m => (m.Id == id));

                return marker;
            }
        }


        public List<Marker> All(int nodeId)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var markers = db.Markers
                    .Include(x => x.Opinions)
                    .Include(x => x.Opinions.Select(o => o.Author))
                    .Include(x => x.Opinions.Select(o => o.Replies))
                    .Where(m => m.Node.Id == nodeId);
                return markers.ToList();
            }
        }


        public Marker Create(MarkerDto dto)
        {
            using (var db = ApplicationDbContext.Create())
            {

                var userId = db.CurrentUserId();
                var user = db.Users.Single(x => x.Id == userId);

                if (dto.Id == null)
                {
                    var marker = new Marker
                    {
                        Height = dto.Height,
                        Width = dto.Width,
                        X = dto.X,
                        Y = dto.Y,
                        Number = db.Markers
                                        .Include(x => x.Node)
                                        .Where(x => x.Node.Id == dto.NodeId)
                                        .Count() + 1,
                        Node = db.Nodes.SingleOrDefault(n => n.Id == dto.NodeId)
                    };

                    var opinion = new MarkerOpinion
                    {
                        Marker = marker,
                        Author = user,
                        Opinion = dto.Text,
                        Timestamp = DateTime.Now
                    };

                    marker.Opinions.Add(opinion);

                    db.MarkersOpinions.Add(opinion);
                    db.Markers.Add(marker);
                    db.SaveChanges();

                    return marker;
                }
                else
                {
                    var marker = db.Markers.Single(x => x.Id == dto.Id);

                    var opinion = new MarkerOpinion
                    {
                        Marker = marker,
                        Author = user,
                        Opinion = dto.Text,
                        Timestamp = DateTime.Now
                    };

                    marker.Opinions.Add(opinion);

                    db.MarkersOpinions.Add(opinion);
                    db.SaveChanges();
                    return marker;

                }
            }
        }


        public bool Delete(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var marker = db.Markers.SingleOrDefault(m => m.Id == id);
                db.Markers.Remove(marker);
                return db.SaveChanges() == 0;

            }
        }


        public void ReplyToOpinion(int MarkerOpinionId, string text)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var loggedUserId = db.CurrentUserId();
                var loggedUser = db.Users.Single(x => x.Id == loggedUserId);

                var opinion = db.MarkersOpinions.Single(x => x.Id == MarkerOpinionId);

                var reply = new MarkerOpinionReply
                {
                    Opinion = opinion,
                    Text = text,
                    Author = loggedUser,
                    Timestamp = DateTime.Now
                };

                opinion.Replies.Add(reply);
                db.MarkersOpinionsReplies.Add(reply);
                db.SaveChanges();
            }
        }

    }


}