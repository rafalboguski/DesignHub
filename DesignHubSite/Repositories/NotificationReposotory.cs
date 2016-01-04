using DesignHubSite.ExtensionMethods;
using DesignHubSite.Models;
using DesignHubSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace DesignHubSite.Repositories
{

    public interface INotificationReposotory
    {

        Notification Get(int id);
        List<Notification> GetAll(int projectId);
        Notification Create(Notification model);
        void Seen(int id);
    }

    public class NotificationReposotory : INotificationReposotory
    {

        private ApplicationDbContext _db = ApplicationDbContext.Create();


        public Notification Get(int id)
        {
            var notification = _db.Notifications.Include(x => x.Author).Single(x => x.Id == id);
            return notification;
        }

        public List<Notification> GetAll(int projectId)
        {
            var notifications = _db.Notifications
                .Include(x => x.Author)
                .Where(x => x.ProjectId == projectId)
                .OrderByDescending(x => x.CreateDate)
                .ToList();
            return notifications;
        }

        public Notification Create(Notification model)
        {
            var userId = _db.CurrentUserId();

            var notification = new Notification
            {
                Author = _db.Users.Single(x => x.Id == userId),
                Header = model.Header,
                Priority = model.Priority,
                visited = false,
                CreateDate = DateTime.Now,
                Link = model.Link,
                ProjectId = model.ProjectId
            };

            _db.Notifications.Add(notification);
            _db.SaveChanges();
            return notification;
        }


        public void Seen(int id)
        {
            var notification = Get(id);
            notification.visited = !notification.visited;
            _db.SaveChanges();

        }
    }
}