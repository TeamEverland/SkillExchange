namespace SkillExchange.Web.Hubs
{
    using System.Data.Entity;
    using System.Linq;

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
                        .Count(n => n.Reciever.UserName == username && n.IsRead == false);

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
                        .Count(n => n.Reciever.UserName == client && n.IsRead == false);

                Clients.Client(connection).appendMessagesCount(messagesCount);
            }
        }
    }
}