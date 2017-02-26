using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RPEFN.UI.Startup))]
namespace RPEFN.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
