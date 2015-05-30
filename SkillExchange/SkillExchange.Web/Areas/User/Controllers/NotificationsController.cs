namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Web.Mvc;
    using Data.Data;
    using Web.Controllers;

    public class NotificationsController : BaseController
    {
        public NotificationsController(ISkillExchangeData data)
            : base(data)
        {
            
        }

        // GET: User/Notifications/Index
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}