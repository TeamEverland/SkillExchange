namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.EnterpriseServices;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.Ajax.Utilities;

    using Data.Data;
    using Models;
    using SkillExchange.Models;
    using Web.Controllers;

    public class MessagesController : BaseController
    {
        public MessagesController(ISkillExchangeData data)
            : base(data)
        {
        }

        // GET: User/Messages/Index
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        // POST: User/Messages/Send
        [HttpPost]
        [Authorize]
        public ActionResult Send(MessageInputModel message)
        {
            if (ModelState.IsValid)
            {
                this.Data.Messages.Add(
                new Message
                {
                    RecieverId = message.RecieverId,
                    SenderId = this.UserProfile.Id,
                    Content = message.Content
                });

                this.Data.SaveChanges();
            }

            var reciever = this.Data.Users.All().First(u => u.Id == message.RecieverId).UserName;

            return this.RedirectToAction("Show", "Profile", new { username = reciever });
        }

        [ChildActionOnly]
        public PartialViewResult Conversations()
        {
            var conversations = this.Data.Messages
                .All()
                .Where(m => m.RecieverId == this.UserProfile.Id)
                .OrderByDescending(m => m.Date)
                .GroupBy(m => m.Sender.UserName)
                .Select(m => new ConversationSummaryViewModel
                {
                    InterlocutorName = m.Key,
                    HasNewMessages = m.Any(x => !x.IsRead)
                });

            return this.PartialView("_Conversations", conversations);
        }

        [ChildActionOnly]
        public PartialViewResult Conversation(string username = null)
        {
            return this.PartialView("_Conversation");
        }
    }
}