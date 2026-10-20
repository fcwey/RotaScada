namespace WebAPI.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    namespace WebAPI.Hubs
    {
        public class ScadaHub : Hub
        {
            public async Task JoinFurnaceGroup(string furnaceId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, furnaceId);
            }

            public async Task LeaveFurnaceGroup(string furnaceId)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, furnaceId);
            }
        }
    }
}
