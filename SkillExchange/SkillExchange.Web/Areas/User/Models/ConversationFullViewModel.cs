namespace SkillExchange.Web.Areas.User.Models
{
    using System.Collections.Generic;

    public class ConversationFullViewModel
    {
        public string InterlocutorName { get; set; }

        public ICollection<MessageViewModel> Messages { get; set; } 
    }
}