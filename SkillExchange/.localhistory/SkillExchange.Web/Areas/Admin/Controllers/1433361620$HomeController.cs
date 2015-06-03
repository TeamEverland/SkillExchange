namespace SkillExchange.Web.Areas.Admin.Controllers
{
    using SkillExchange.Data.Data;
    using SkillExchange.Web.Controllers;

    public class HomeController : BaseController
    {
        protected HomeController(ISkillExchangeData data)
            : base(data)
        {
        }
    }
}