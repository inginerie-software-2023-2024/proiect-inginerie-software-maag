using Microsoft.AspNetCore.SignalR;

namespace Petbook.Hubs
{
    public class ChatHub : Hub
    {

        private readonly IUserConnectionManager _connectionManager;

        public ChatHub(IUserConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("_+++++++++++++++_++++++++++++++_+++++++++++++");
            var userId = Context.UserIdentifier;
            _connectionManager.AddConnection(userId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectionManager.RemoveConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverUserId, string message, string messageDate)
        {
            var connectionId = _connectionManager.GetConnection(receiverUserId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.UserIdentifier, message, messageDate);
            }
        }
    }
}
