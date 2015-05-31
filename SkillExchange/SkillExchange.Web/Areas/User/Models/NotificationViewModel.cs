namespace SkillExchange.Web.Areas.User.Models
{
    using System;

    public class NotificationViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public bool IsRead { get; set; }
    }
}