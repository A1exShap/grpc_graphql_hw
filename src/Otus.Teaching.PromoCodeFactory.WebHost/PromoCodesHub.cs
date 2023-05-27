using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.SignalR
{
    public class PromoCodesHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            
            await Clients.Caller.SendAsync("Message", "Connected successfully!");
        }

        public async Task SendUser(string username, string message)
        {
            await Clients.User(username).SendAsync("Message", username, message);
        }

        //public async Task SubscribeToBoard(string partnerName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, partnerName);
        //    await Clients.Caller.SendAsync("Message", "Subscribe to messages successfully!");
        //}
    }
}