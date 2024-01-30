using System.Collections.Concurrent;


namespace Petbook.Hubs
{
    public interface IUserConnectionManager
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string connectionId);
    string GetConnection(string userId);
}

    public class UserConnectionManager : IUserConnectionManager
    {
        private readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public void AddConnection(string userId, string connectionId)
        {
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(userId + " " + connectionId);
            _connections[userId] = connectionId;
            Console.WriteLine(_connections[userId]);
        }

        public void RemoveConnection(string connectionId)
        {
            var item = _connections.FirstOrDefault(kvp => kvp.Value == connectionId);
            if (!item.Equals(default(KeyValuePair<string, string>)))
            {
                _connections.Remove(item.Key);
            }
        }

        public string GetConnection(string userId)
        {
            _connections.TryGetValue(userId, out var connectionId);
            return connectionId;
        }
    }

}