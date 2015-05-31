namespace SkillExchange.Web.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Context;
    using Data.Data;
    using Microsoft.AspNet.SignalR;

    public class BaseHub : Hub
    {
        protected ConnectionMapping<string> usersConnections =
            new ConnectionMapping<string>();

        protected ISkillExchangeData data =
            new SkillExchangeData(new SkillExchangeDbContext());

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            usersConnections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            usersConnections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!usersConnections.GetConnections(name).Contains(Context.ConnectionId))
            {
                usersConnections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}