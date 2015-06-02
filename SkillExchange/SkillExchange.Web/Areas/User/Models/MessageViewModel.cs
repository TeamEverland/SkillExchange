namespace SkillExchange.Web.Areas.User.Models
{
    using System;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public string SenderId { get; set; }

        public string SenderName { get; set; }

        public bool IsRead { get; set; }
    }
}