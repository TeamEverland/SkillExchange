namespace SkillExchange.Web.Hubs
{
    using System.Data.Entity;
    using System.Linq;

    public class NotificationsHub : BaseHub
    {
        public void EstimateNotificationsCount()
        {
            var usernames = usersConnections.GetUsernames();
            foreach (var username in usernames)
            {
                var userConnections = usersConnections.GetConnections(username);
                foreach (var connection in userConnections)
                {
                    var notificationsCount = data.Notifications
                        .All()
                        .Include(n => n.Reciever)
                        .Count(n => n.Reciever.UserName == username && n.IsRead == false);

                    Clients.Client(connection).appendNotificationsCount(notificationsCount);
                }
            }
        }

        public void EstimateNotificationsCountForClient(string client)
        {
            var userConnections = usersConnections.GetConnections(client);
            foreach (var connection in userConnections)
            {
                var notificationsCount = data.Notifications
                        .All()
                        .Include(n => n.Reciever)
                        .Count(n => n.Reciever.UserName == client && n.IsRead == false);

                Clients.Client(connection).appendNotificationsCount(notificationsCount);
            }
        }
    }
}