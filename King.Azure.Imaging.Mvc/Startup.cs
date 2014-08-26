using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(King.Azure.Imaging.Mvc.Startup))]
namespace King.Azure.Imaging.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}