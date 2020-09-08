using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GAFPAY.Startup))]
namespace GAFPAY
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
