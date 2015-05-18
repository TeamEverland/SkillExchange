using Microsoft.Owin;

[assembly: OwinStartupAttribute(typeof(SkillExchange.Web.Startup))]
namespace SkillExchange.Web
{
    using Owin;
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
