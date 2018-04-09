using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CommunityPro.Startup))]
namespace CommunityPro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
