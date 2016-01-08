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
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IApplicationDbContext<ApplicationUser>, ApplicationDbContext>( new ContainerControlledLifetimeManager());
            container.RegisterType<IRepository<Project>, ProjectRepository>();
            container.RegisterType<INodeRepository, NodeRepository>();
            container.RegisterType<IMarkerRepository, MarkerRepository>();
            container.RegisterType<IPermissionRepository, PermissionRepository>();
            container.RegisterType<INotificationReposotory, NotificationReposotory>();

            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IProjectDetailsService, ProjectDetailsService>();
            container.RegisterType<IPermissionsService, PermissionsService>();
            container.RegisterType<IUsersService, UsersService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}