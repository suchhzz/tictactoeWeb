using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace tictactoeweb.Hubs
{
    public class FindGameHub : Hub
    {
        public static int PlayersCount { get; set; } = 0;
        public static int GroupId { get; set; } = 0;
        public override async Task OnConnectedAsync()
        {

            PlayersCount++;

            GetId();

            await Clients.All.SendAsync("PlayerJoin", PlayersCount);

            checkPlayersReady();

            await base.OnConnectedAsync();
        }

        private async void checkPlayersReady()
        {
            if (PlayersCount == 2)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, GroupId.ToString()); 

                await Clients.All.SendAsync("PlayersReady", true, GroupId);

                GroupId++;
            }
        }

        private async void GetId()
        {
            await Clients.Caller.SendAsync("GetId", PlayersCount);
        }
    }
}
