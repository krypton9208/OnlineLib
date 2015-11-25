using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineLib.App.Startup))]
namespace OnlineLib.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
