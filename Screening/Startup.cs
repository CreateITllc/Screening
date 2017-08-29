using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Screening.Startup))]
namespace Screening
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
