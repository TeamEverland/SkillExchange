namespace SkillExchange.Web.Hubs
{
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.Ajax.Utilities;

    public class MessagesHub : BaseHub
    {
        public void EstimateMessagesCount()
        {
            var usernames = usersConnections.GetUsernames();
            foreach (var username in usernames)
            {
                var userConnections = usersConnections.GetConnections(username);
                foreach (var connection in userConnections)
                {
                    var messagesCount = data.Messages
                        .All()
                        .Include(n => n.Reciever)
                        .Count(m => m.Reciever.UserName == username && m.IsRead == false);

                    Clients.Client(connection).appendMessagesCount(messagesCount);
                }
            }
        }

        public void EstimateMessagesCountForClient(string client)
        {
            var userConnections = usersConnections.GetConnections(client);
            foreach (var connection in userConnections)
            {
                var messagesCount = data.Messages
                        .All()
                        .Include(n => n.Reciever)
                        .Count(m => m.Reciever.UserName == client && m.IsRead == false);

                Clients.Client(connection).appendMessagesCount(messagesCount);
            }
        }

        public void MarkMessagesAsRead(string username, string interlocutor)
        {
            var messages = this.data.Messages
                .All()
                .Where(
                    m => (m.Sender.UserName == interlocutor &&
                        m.Reciever.UserName == username));

            foreach (var message in messages)
            {
                message.IsRead = true;
                this.data.Messages.Update(message);
            }

            this.data.SaveChanges();

            this.EstimateMessagesCountForClient(username);
        }
    }
}