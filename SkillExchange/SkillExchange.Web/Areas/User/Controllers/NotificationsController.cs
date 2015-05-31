﻿namespace SkillExchange.Web.Areas.User.Controllers
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
                .Where(n => n.Reciever.Id == this.UserProfile.Id)
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

        // POST: User/Notifications/MarkAsRead
        [Authorize]
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

            return this.RedirectToAction("Index", new {controller = "Notifications", area = "User"});
        }
    }
}