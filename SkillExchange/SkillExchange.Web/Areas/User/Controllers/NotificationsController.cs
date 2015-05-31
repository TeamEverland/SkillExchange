namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Data.Data;
    using Models;
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
            var notifications = this.Data.Notifications
                .All()
                .Where(n => n.Reciever.UserName == this.Profile.UserName)
                .Select(n => new NotificationViewModel
                {
                    Id = n.Id,
                    Content = n.Content,
                    IsRead = n.IsRead,
                    Date = n.Date
                })
                .OrderByDescending(n => n.Date);

            return this.View(notifications);
        }
    }
}