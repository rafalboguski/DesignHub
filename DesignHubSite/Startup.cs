using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DesignHubSite.Startup))]
namespace DesignHubSite
{
    public partial class Startup
    {
        //todo: no redirect to login/register if using api

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
