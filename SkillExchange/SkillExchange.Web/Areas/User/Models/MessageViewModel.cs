namespace SkillExchange.Web.Areas.User.Models
{
    using System;
    using System.Linq.Expressions;

    using SkillExchange.Models;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public string SenderId { get; set; }

        public string SenderName { get; set; }

        public bool IsRead { get; set; }

        public static Expression<Func<Message, MessageViewModel>> ViewModel
        {
            get
            {
                return e => new MessageViewModel
                {
                    Date = e.Date,
                    IsRead = e.IsRead,
                    SenderId = e.SenderId,
                    SenderName = e.Sender.UserName,
                    Content = e.Content
                };
            }
        } 
    }
}