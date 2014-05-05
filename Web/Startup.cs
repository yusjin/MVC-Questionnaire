using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YQ.Web.Startup))]
namespace YQ.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
