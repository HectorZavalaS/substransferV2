using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(substransferV2.Startup))]
namespace substransferV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
