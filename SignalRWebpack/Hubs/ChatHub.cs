using Microsoft.AspNetCore.SignalR;

namespace SignalRWebpack.Hubs
{
    public class ChatHub: Hub
    {
        public Task NewMessage(long username, string message)
            => Clients.All.SendAsync("messageReceived", username, message);
    }
}