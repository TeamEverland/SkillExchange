namespace SkillExchange.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using SkillExchange.Data.Data;
    using SkillExchange.Web.Controllers;

    public class HomeController : BaseController
    {
        public HomeController(ISkillExchangeData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {

            return this.View();
        }
    }
}