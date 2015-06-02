namespace SkillExchange.Web.Areas.User.Models
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.Ajax.Utilities;
    using SkillExchange.Models;

    public class NotificationViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public bool IsRead { get; set; }

        public static Expression<Func<Notification, NotificationViewModel>> ViewModel
        {
            get
            {
                return e => new NotificationViewModel
                {
                    Id = e.Id,
                    Content = e.Content,
                    IsRead = e.IsRead,
                    Date = e.Date
                };
            }
        }
    }
}