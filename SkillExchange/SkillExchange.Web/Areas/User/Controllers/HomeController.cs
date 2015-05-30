namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Web.Mvc;
    using Data.Data;
    using Web.Controllers;

    public class HomeController : BaseController
    {
        public HomeController(ISkillExchangeData data)
            : base(data)
        {
        }

        // GET: User/Home/Index
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}