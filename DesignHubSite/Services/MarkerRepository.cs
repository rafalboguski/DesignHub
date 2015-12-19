using System;
using System.Collections.Generic;
using System.Linq;
using DesignHubSite.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using DesignHubSite.ExtensionMethods;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesignHubSite.Services
{

    public interface IMarkerRepository
    {

        Marker Single(int id);

        List<Marker> All(int nodeId);

        Marker Create(MarkerDto model);

        bool Delete(int id);

    }


    public class MarkerRepository : IMarkerRepository
    {

        public Marker Single(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var marker = db.Markers.SingleOrDefault(m => (m.Id == id));

                return marker;
            }
        }


        public List<Marker> All(int nodeId)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var markers = db.Markers.Where(m => m.Node.Id == nodeId);
                return markers.ToList();
            }
        }

    
        public Marker Create(MarkerDto model)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var marker = new Marker
                {
                    Height = model.Height,
                    Width = model.Width,
                    X = model.X,
                    Y = model.Y,
                    text = model.Text,
                    Timestamp = DateTime.Now,
                    Node = db.Nodes.SingleOrDefault(n => n.Id == model.NodeId)
                };

                db.Markers.Add(marker);
                db.SaveChanges();

                return marker;
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


    }


}