namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Web.Mvc;
    using Data.Data;
    using Web.Controllers;

    public class Profile : BaseController
    {
        public Profile(ISkillExchangeData data)
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