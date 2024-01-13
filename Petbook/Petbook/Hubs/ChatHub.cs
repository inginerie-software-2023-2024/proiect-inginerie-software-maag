using Microsoft.AspNetCore.SignalR;

namespace Petbook.Hubs
{
    public class ChatHub : Hub
    {

        private static string _currentConnectionId = string.Empty;

        public override Task OnConnectedAsync()
        {
            if (string.IsNullOrEmpty(_currentConnectionId))
            {
                _currentConnectionId = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
