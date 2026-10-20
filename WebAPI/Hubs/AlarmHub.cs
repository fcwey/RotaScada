using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public class AlarmHub : Hub
    {
        public async Task SubscribeToAlarms()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "AlarmSubscribers");
        }
    }
}
