using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using tictactoeweb.Context;
using tictactoeweb.Models.GameModels;
using tictactoeweb.Models.HomeModels;
using tictactoeweb.Models.MainModels;
using tictactoeweb.Services;

namespace tictactoeweb.Hubs
{
    public class TicTacHub : Hub // refact
    {
        private UserServices _services;
        private RoomService _roomService;
        private readonly ILogger<TicTacHub> _logger;
        public TicTacHub(ILogger<TicTacHub> logger, UserServices services, RoomService roomService)
        {
            _logger = logger;
            _services = services;
            _roomService = roomService;
        }
        public static int Id { get; set; } = 1;
        public static int UsersOnline { get; set; } = 0;
        public static int UsersAll { get; set; } = 0;
        public static int JoinedUsers { get; set; } = 0;

        public async Task SetUserId()
        {
            await Clients.Caller.SendAsync("GetUserId", Id);
            switchId();
        }

        public async Task SendPlayerMove(string roomId, int userId, int cell, int winIndex)
        {
            Room currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            currentRoom.MoveCounter++;

            if (winIndex != 0 || currentRoom.MoveCounter == 9)
            {

                if (currentRoom.MoveCounter == 9)
                {
                    winIndex = 3;
                }

                _logger.LogInformation("game over");

                await GameOver(roomId, winIndex);
            }
            else
            {
                _roomService.switchPassMoveId(Guid.Parse(roomId), userId);

                await Clients.Group(roomId).SendAsync("PassMove", currentRoom.PassMoveID);

                _logger.LogInformation("game continues");
            }

            await Clients.Group(roomId).SendAsync("ReceiveMove", userId, cell, winIndex);
        }

        public async Task GameOver(string roomId, int winIndex)
        {
            await Clients.Group(roomId).SendAsync("GameStatus", winIndex == 0);

            await GetWinMessage(roomId, winIndex);

            await SetWinner(roomId, winIndex);

            await _roomService.RemoveRoomById(Guid.Parse(roomId));
        }

        public async Task GetWinMessage(string roomId, int winIndex)
        {
            string winner = "";
            Room currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            _logger.LogInformation("GetWinMessage (inside), winIndex = " + winIndex);


            switch (winIndex)
            {
                case 1:
                    winner = $"player: {currentRoom.Players[0].Username} won";
                    break;
                case 2:
                    winner = $"player: {currentRoom.Players[0].Username} won";
                    break;
                case 3:
                    winner = "DRAW";
                    break;
            }

            _logger.LogInformation($"GetWinMessage (inside) winIndex = {winIndex} | message = {winner}");

            await Clients.Group(roomId).SendAsync("WinMessage", winner);

            _logger.LogInformation("message sended: " + winner);
        }

        private async Task SetWinner(string roomId, int winIndex)
        {
            Room currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            var firstPlayer = await _services.GetUserById(currentRoom.Players[0].Id);

            var secondPlayer = await _services.GetUserById(currentRoom.Players[1].Id);

            if (winIndex == 1)
            {
                firstPlayer.Wins++;
            }
            else if (winIndex == 2)
            {
                secondPlayer.Wins++;
            }

            firstPlayer.Games++;
            secondPlayer.Games++;

            await _services.UpdateUser(firstPlayer);
            await _services.UpdateUser(secondPlayer);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            UsersOnline--;

            await Clients.All.SendAsync("GetOnlineUsers", UsersOnline, UsersAll);

            await base.OnDisconnectedAsync(exception);
        }

        public static void switchId()
        {
            if (Id == 1)
            {
                Id = 2;
            }
            else
            {
                Id = 1;
            }
        }
        public override async Task OnConnectedAsync()
        {
            UsersOnline++;
            UsersAll++;

            await Clients.Caller.SendAsync("GetUserId", Id);
            switchId();

            await Clients.All.SendAsync("GetOnlineUsers", UsersOnline, UsersAll);

            await base.OnConnectedAsync();
        }

        public async Task SetPlayersDefault(string roomId)
        {
            Room currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            await Clients.Group(roomId).SendAsync("GetCurrentPlayer", currentRoom.PassMoveID);

            await Clients.Group(roomId).SendAsync("PassMove", currentRoom.PassMoveID);

        }

        public async Task JoinPlayer(string inputId)
        {
            Guid currentUserId = Guid.Parse(inputId);

            if (_roomService.Rooms[_roomService.Rooms.Count - 1].Players[JoinedUsers].Id != currentUserId)
            {
                _logger.LogInformation($"user id {inputId} is not {_roomService.Rooms[_roomService.Rooms.Count - 1].Players[JoinedUsers].Id}");
            }
            else
            {
                _logger.LogInformation($"user id {inputId} connected");

                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    _roomService.Rooms[_roomService.Rooms.Count - 1].RoomId.ToString() );
            }

            JoinedUsers++;

            if (JoinedUsers == 2)
            {
                await GetGroupId(_roomService.Rooms[_roomService.Rooms.Count - 1].RoomId.ToString());

                await Clients.Group
                    (   _roomService.Rooms[_roomService.Rooms.Count - 1]
                        .RoomId.ToString()
                    ).SendAsync("PlayersReady", true);

                JoinedUsers = 0;
            }
        }

        public async Task GetGroupId(string groupId)
        {
            await Clients.Groups(groupId).SendAsync("GetGroupId", groupId);
        }
    }
}
