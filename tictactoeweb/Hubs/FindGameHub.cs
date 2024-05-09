using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using tictactoeweb.Models.GameModels;
using tictactoeweb.Models.MainModels;
using tictactoeweb.Services;

namespace tictactoeweb.Hubs
{
    public class FindGameHub : Hub
    {
        private RoomService _roomService;
        private UserService _userService;
        public FindGameHub(RoomService roomService, UserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }

        public static int UsersCount { get; set; } = 0;
        public static int GroupId { get; set; } = 0;
        public static List<User> ConnectedUsers = new List<User>();

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            UsersCount = 0;

            await Clients.All.SendAsync("PlayerJoin", UsersCount);

            await base.OnDisconnectedAsync(exception);
        }

        private async void checkPlayersReady()
        {
            if (UsersCount == 2)
            {
                await _roomService.CreateRoom(ConnectedUsers);

                string receivedRoomId = _roomService.Rooms[_roomService.Rooms.Count - 1].RoomId.ToString();

                await Clients.All.SendAsync("PlayersReady", receivedRoomId);

                ConnectedUsers.Clear();

                GroupId++;

                UsersCount = 0;
            }
        }

        public async Task AddPlayerToList(string userId)
        {
            UsersCount++;

            await Clients.All.SendAsync("PlayerJoin", UsersCount);

            var user = await _userService.GetUserById(Guid.Parse(userId));

            ConnectedUsers.Add(user);

            checkPlayersReady();
        }
    }
}
