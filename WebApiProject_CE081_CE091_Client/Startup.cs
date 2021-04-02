using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WCFProject_CE091_CE081_Client.Startup))]
namespace WCFProject_CE091_CE081_Client
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
