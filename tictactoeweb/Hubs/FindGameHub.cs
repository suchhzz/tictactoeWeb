using Microsoft.AspNetCore.SignalR;

namespace tictactoeweb.Hubs
{
    public class FindGameHub : Hub
    {
        public static int PlayersCount { get; set; } = 0;
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
            await Clients.All.SendAsync("PlayersReady", PlayersCount == 2);
        }

        private async void GetId()
        {
            await Clients.Caller.SendAsync("GetId", PlayersCount);
        }
    }
}
