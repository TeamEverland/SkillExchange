namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Web.Controllers;
    using Data.Data;
    using Hubs;
    using Microsoft.AspNet.SignalR;
    using Models;


    public class NotificationsController : BaseController
    {
        public NotificationsController(ISkillExchangeData data)
            : base(data)
        {
        }

        //
        // GET: User/Notifications/Index
        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            var notifications = this.Data.Notifications
                .All()
                .Where(n => n.Reciever.Id == this.UserProfile.Id)
                .Select(NotificationViewModel.ViewModel)
                .OrderByDescending(n => n.IsRead);

            return this.View(notifications);
        }

        // POST: User/Notifications/MarkAsRead
        [System.Web.Mvc.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAsRead(int notificationId)
        {
            var notification = this.Data.Notifications
                .All()
                .FirstOrDefault(n => n.Id == notificationId);

            if (notification != null)
            {
                if (notification.RecieverId == this.UserProfile.Id)
                {
                    notification.IsRead = true;
                    this.Data.SaveChanges();
                }
                else
                {
                    // TODO Add error for unauthorized action
                }
            }
            else
            {
                // TODO Add error in temp data
            }

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            hubContext.Clients.All.estimateNotificationsCountForClient(this.UserProfile.UserName);

            return this.RedirectToAction("Index", new { controller = "Notifications", area = "User" });
        }
    }
}