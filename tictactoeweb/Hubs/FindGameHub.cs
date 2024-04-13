using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using tictactoeweb.Models.GameModels;
using tictactoeweb.Models.HomeModels;
using tictactoeweb.Models.MainModels;
using tictactoeweb.Services;

namespace tictactoeweb.Hubs
{
    public class FindGameHub : Hub
    {
        private RoomService _roomService;
        private UserServices _userService;
        public FindGameHub(RoomService roomService, UserServices userService)
        {
            _roomService = roomService;
            _userService = userService;
        }

        public static int PlayersCount { get; set; } = 0;
        public static int GroupId { get; set; } = 0;
        public static List<UserViewModel> Players = new List<UserViewModel>();
        public override async Task OnConnectedAsync()
        {
            PlayersCount++;

            await Clients.Caller.SendAsync("GetId", PlayersCount);

            await Clients.All.SendAsync("PlayerJoin", PlayersCount);


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            PlayersCount = 0;

            await Clients.All.SendAsync("PlayerJoin", PlayersCount);

            await base.OnDisconnectedAsync(exception);
        }

        private async void checkPlayersReady()
        {
            if (PlayersCount == 2)
            {
                await Clients.All.SendAsync("PlayersReady", true, GroupId);

                await _roomService.CreateRoom(Players);

                Players.Clear();

                GroupId++;

                PlayersCount = 0;
            }
        }

        public async Task AddPlayerToList(string userId)
        {
            var user = await _userService.GetUserById(Guid.Parse(userId));

            UserViewModel userVM = new UserViewModel { Id = user.Id, Username = user.Username };

            Players.Add(userVM);

            checkPlayersReady();
        }
    }
}
