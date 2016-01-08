using Microsoft.Practices.Unity;
using System.Web.Http;
using DesignHubSite.Repositories;
using Unity.WebApi;
using DesignHubSite.Models;
using DesignHubSite.Services;

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

            MvcApplication.Container.RegisterType<IApplicationDbContext<ApplicationUser>, ApplicationDbContext>( new ContainerControlledLifetimeManager());
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
}