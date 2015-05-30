namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Web.Mvc;
    using Data.Data;
    using Web.Controllers;

    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ISkillExchangeData data)
            : base(data)
        {
        }

        // GET: User/Home/Index
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        public PartialViewResult Categories()
        {
            return this.PartialView("_Categories");
        }
    }
}