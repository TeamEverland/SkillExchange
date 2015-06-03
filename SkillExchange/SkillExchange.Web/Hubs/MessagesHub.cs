namespace SkillExchange.Web.Hubs
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class MessagesHub : BaseHub
    {
        private static HashSet<int> MessagesAppended = new HashSet<int>();
 
        public void EstimateMessagesCount()
        {
            var usernames = usersConnections.GetUsernames();
            foreach (var username in usernames)
            {
                var userConnections = usersConnections.GetConnections(username);
                foreach (var connection in userConnections)
                {
                    var senders = data.Messages
                        .All()
                        .Include(n => n.Reciever)
                        .Where(m => m.Reciever.UserName == username && m.IsRead == false)
                        .GroupBy(m => m.SenderId)
                        .Select(m => m.Key);

                    var messagesCount = senders.Count();
                    Clients.Client(connection).appendMessagesCount(messagesCount);
                }
            }
        }

        public void EstimateMessagesCountForClient(string client)
        {
            var userConnections = usersConnections.GetConnections(client);
            foreach (var connection in userConnections)
            {
                var senders = data.Messages
                        .All()
                        .Include(n => n.Reciever)
                        .Where(m => m.Reciever.UserName == client && m.IsRead == false)
                        .GroupBy(m => m.SenderId)
                        .Select(m => m.Key);

                var messagesCount = senders.Count();

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

        public void FindMessageRecieverClients(string reciever, int messageId)
        {
            var sender = this.data.Messages.Find(messageId).Sender.UserName;

            if (!MessagesAppended.Contains(messageId))
            {
                var userConnections = usersConnections.GetConnections(reciever);
                foreach (var connection in userConnections)
                {
                    Clients.Client(connection).appendNewMessage(messageId, sender);
                    Clients.Client(connection).appendConversationSummary(sender);
                }

                MessagesAppended.Add(messageId);
            }
        }
    }
}