namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Data.Data;
    using Hubs;
    using Microsoft.AspNet.SignalR;
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
        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        // POST: User/Messages/Send
        [HttpPost]
        [System.Web.Mvc.Authorize]
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
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            hubContext.Clients.All.estimateMessagesCountForClient(reciever);

            return this.RedirectToAction("Index", "Messages", new { area = "User" });
        }

        // POST: User/Messages/SendAsync
        [HttpPost]
        [System.Web.Mvc.Authorize]
        public PartialViewResult SendAsync(MessageInputModel message)
        {
            Message newMessage = new Message();
            if (ModelState.IsValid)
            {

                newMessage.RecieverId = message.RecieverId;
                newMessage.SenderId = this.UserProfile.Id;
                newMessage.Content = message.Content;

                this.Data.Messages.Add(newMessage);

                this.Data.SaveChanges();
            }

            var messageSent = this.Data.Messages
                .All()
                .Where(m => m.Id == newMessage.Id)
                .Select(MessageViewModel.ViewModel)
                .First();

            var reciever = this.Data.Users.Find(message.RecieverId);
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            hubContext.Clients.All.estimateMessagesCountForClient(reciever.UserName);

            return this.PartialView("_Message", messageSent);
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

        public ActionResult Conversation(string interlocutorName)
        {
            var interlocutor = this.Data.Users
                .All()
                .FirstOrDefault(u => u.UserName == interlocutorName);

            if (interlocutor != null)
            {
                var conversationMessages = this.Data.Messages
                .All()
                .Where(
                    m => (m.SenderId == interlocutor.Id &&
                        m.RecieverId == this.UserProfile.Id) ||
                        m.SenderId == this.UserProfile.Id &&
                        m.RecieverId == interlocutor.Id)
                .OrderBy(m => m.Date)
                .Select(MessageViewModel.ViewModel)
                .ToList();

                var viewModel = new ConversationFullViewModel
                {
                    InterlocutorId = interlocutor.Id,
                    Messages = conversationMessages
                };

                return this.PartialView("_Conversation", viewModel);
            }
            else
            {
                TempData["message"] = new NotificationMessage
                {
                    Content = "Sorry, message reciever not found",
                    Type = NotificationMessageType.Error
                };

                return this.RedirectToAction("Error", "Home", new {area = "User"});
            } 
        }
    }
}