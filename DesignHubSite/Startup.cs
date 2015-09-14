using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DesignHubSite.Startup))]
namespace DesignHubSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
