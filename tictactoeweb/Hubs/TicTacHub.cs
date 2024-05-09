using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using tictactoeweb.Context;
using tictactoeweb.Models.GameModels;
using tictactoeweb.Models.MainModels;
using tictactoeweb.Services;

namespace tictactoeweb.Hubs
{
    public class TicTacHub : Hub
    {
        private readonly RoomService _roomService;
        private readonly GameService _gameService;
        private readonly UserService _userService;
        private readonly ILogger<TicTacHub> _logger;
        public TicTacHub(ILogger<TicTacHub> logger, RoomService roomService, GameService gameService, UserService userService)
        {
            _logger = logger;
            _roomService = roomService;
            _gameService = gameService;
            _userService = userService;
        }
        public async Task PlayerMove(string roomId, int cell)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            await Clients.Group(roomId).SendAsync("ReceiveMove", currentRoom.Playground.PassMoveId, cell);

            _gameService.SetPlayerMove(currentRoom, cell);

            await Clients.Group(roomId).SendAsync("ChangePlayerTurn", currentRoom.Playground.PassMoveId);

            if (currentRoom.Playground.WinIndex != 0 || currentRoom.Playground.MoveCounter == 9)
            {

                if (currentRoom.Playground.MoveCounter == 9)
                {
                    currentRoom.Playground.WinIndex = 3;
                }

                await Clients.Group(roomId).SendAsync("GameOver", _gameService.getWinMessage(currentRoom));

                _logger.LogInformation("game over");
            }
            else
            {
                _logger.LogInformation("game continues");
            }

        }

        public async Task JoinPlayer(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            _logger.LogInformation("player connected: " + roomId);
        }

        public async Task PlayerSettings(string roomId, string userId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            await Clients.Caller.SendAsync("PlayerId", currentRoom.Players.Where(p => p.Id == Guid.Parse(userId)).First().PlayerId, currentRoom.Playground.PassMoveId);
        }
    }
}
