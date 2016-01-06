using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DesignHubSite.Models;
using DesignHubSite.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DesignHubSite.ExtensionMethods;
using DesignHubSite.Services;

namespace DesignHubSite.Controllers
{
    [Authorize]
    [RoutePrefix("api/Notifications")]
    public class NotificationsController : ApiController
    {

        private readonly INotificationReposotory _repo;

        public NotificationsController(INotificationReposotory repo)
        {
            _repo = repo;
        }


        [Route("project/{projectId}")]
        public IHttpActionResult GetNotifications(int projectId)
        {
            var notifications = _repo.GetAll(projectId);

            var result = notifications
                                    .GroupBy(n => n.CreateDate.Date)
                                    .Select(g => g.ToList())
                                    .ToList();
                                                   
          

            return Json(result);
        }

        [Route("{id}")]
        public Notification GetNotification(int id)
        {
            var notification = _repo.Get(id);
            return notification;
        }

        [HttpPost]
        [Route("{id}/seen")]
        public IHttpActionResult Seen(int id)
        {
            _repo.Seen(id);
            return Ok();

        }

    }
}