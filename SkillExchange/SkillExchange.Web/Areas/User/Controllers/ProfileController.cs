namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Web.Mvc;
    using Data.Data;
    using Web.Controllers;

    public class ProfileController : BaseController
    {
        public ProfileController(ISkillExchangeData data)
            : base(data)
        {
        }

        // GET: User/Profile/Index
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}