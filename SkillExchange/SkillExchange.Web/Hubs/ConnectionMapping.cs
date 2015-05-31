namespace SkillExchange.Web.Hubs
{
    using System.Collections.Generic;
    using System.Linq;

    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> clientsConnections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return clientsConnections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (clientsConnections)
            {
                HashSet<string> connections;
                if (!clientsConnections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    clientsConnections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (clientsConnections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public Dictionary<T, HashSet<string>>.KeyCollection GetUsernames()
        {
            var usernames = clientsConnections.Keys;
            return usernames;
        } 

        public void Remove(T key, string connectionId)
        {
            lock (clientsConnections)
            {
                HashSet<string> connections;
                if (!clientsConnections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        clientsConnections.Remove(key);
                    }
                }
            }
        }
    }
}