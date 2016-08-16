using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shopify.Startup))]
namespace Shopify
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
