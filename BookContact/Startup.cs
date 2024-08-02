using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookContact.Startup))]
namespace BookContact
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
