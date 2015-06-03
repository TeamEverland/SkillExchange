namespace SkillExchange.Web.Areas.User.Models
{
    using System.Web.Mvc;

    public class MessageOutputModel
    {
        public string InterinterlocutorName { get; set; }

        public PartialViewResult MessageView { get; set; }
    }
}