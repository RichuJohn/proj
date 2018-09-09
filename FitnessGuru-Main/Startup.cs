using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FitnessGuru_Main.Startup))]
namespace FitnessGuru_Main
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
