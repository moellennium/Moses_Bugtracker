using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Moses_Bugtracker.Startup))]
namespace Moses_Bugtracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
