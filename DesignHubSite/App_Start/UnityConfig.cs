using Microsoft.Practices.Unity;
using System.Web.Http;
using DesignHubSite.Repositories;
using Unity.WebApi;
using DesignHubSite.Models;
using DesignHubSite.Services;
using System.Web;

namespace DesignHubSite
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            MvcApplication.Container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            MvcApplication.Container.RegisterType<IApplicationDbContext<ApplicationUser>, ApplicationDbContext>(new HttpContextLifetimeManager());
            MvcApplication.Container.RegisterType<IRepository<Project>, ProjectRepository>();
            MvcApplication.Container.RegisterType<INodeRepository, NodeRepository>();
            MvcApplication.Container.RegisterType<IMarkerRepository, MarkerRepository>();
            MvcApplication.Container.RegisterType<IPermissionRepository, PermissionRepository>();
            MvcApplication.Container.RegisterType<INotificationReposotory, NotificationReposotory>();

            MvcApplication.Container.RegisterType<IProjectService, ProjectService>();
            MvcApplication.Container.RegisterType<IProjectDetailsService, ProjectDetailsService>();
            MvcApplication.Container.RegisterType<IPermissionsService, PermissionsService>();
            MvcApplication.Container.RegisterType<IUsersService, UsersService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(MvcApplication.Container);
        }
    }


    public class HttpContextLifetimeManager : LifetimeManager
    {
        private readonly object key = new object();

        public override object GetValue()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.Items.Contains(key))
                return HttpContext.Current.Items[key];
            else
                return null;
        }

        public override void RemoveValue()
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items.Remove(key);
        }

        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[key] = newValue;
        }
    }
}