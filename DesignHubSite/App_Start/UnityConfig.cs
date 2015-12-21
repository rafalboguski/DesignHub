using Microsoft.Practices.Unity;
using System.Web.Http;
using DesignHubSite.Services;
using Unity.WebApi;
using DesignHubSite.Models;

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
            container.RegisterType<IRepository<Project>, ProjectRepository>();
            container.RegisterType<INodeRepository, NodeRepository>();
            container.RegisterType<IMarkerRepository, MarkerRepository>();


            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IProjectDetailsService, ProjectDetailsService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}