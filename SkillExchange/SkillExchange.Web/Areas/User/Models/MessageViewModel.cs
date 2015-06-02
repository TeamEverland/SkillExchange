using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillExchange.Web.Areas.User.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public int SenderId { get; set; }

        public string SenderName { get; set; }

        public bool IsRead { get; set; }
    }
}