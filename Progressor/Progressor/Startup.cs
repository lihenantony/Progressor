using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Progressor.Startup))]
namespace Progressor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
