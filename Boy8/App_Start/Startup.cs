using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Boy8.Startup))]
namespace Boy8
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
