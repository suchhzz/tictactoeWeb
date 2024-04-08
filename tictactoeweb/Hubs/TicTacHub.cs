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
    public class TicTacHub : Hub
    {
        private UserServices _services;
        private readonly ILogger<TicTacHub> _logger;
        public TicTacHub(ILogger<TicTacHub> logger, UserServices services)
        {
            _logger = logger;
            _services = services;
        }
        public static int Id { get; set; } = 0;
        public static int UsersOnline { get; set; } = 0;
        public static int UsersAll { get; set; } = 0;
        public static int MoveCounter { get; set; } = 0;
        public static int PlayerIdCounter { get; set; } = 0;
        public static List<PlayerViewModel> Players = new List<PlayerViewModel>();

        private void ClearHub()
        {
            Id = 0;
            UsersOnline = 0;
            UsersAll = 0;
            MoveCounter = 0;
            PlayerIdCounter = 0;
            Players = new List<PlayerViewModel>();
        }

        public async Task SetUserId()
        {
            Id++;
            await Clients.Caller.SendAsync("GetUserId", Id);
        }

        public int passMovePlayerId { get; set; } = 1;

        public async Task GetPassMove()
        {
            await Clients.All.SendAsync("PassMove", passMovePlayerId);
        }

        public async Task SendPlayerMove(int userId, int cell, int winIndex)
        {
            MoveCounter++;

            if (winIndex != 0 || MoveCounter == 9)
            {

                if (MoveCounter == 9)
                {
                    winIndex = 3;
                }


                _logger.LogInformation("game over");

                await GameOver(winIndex);
            }
            else
            {
                switchPassMoveId(userId);
                await GetPassMove();

                _logger.LogInformation("game continues");
            }

            await Clients.All.SendAsync("ReceiveMove", userId, cell, winIndex);
        }

        public async Task GameOver(int winIndex)
        {
            await GetGameStatus(winIndex);

            _logger.LogInformation("GetGameStatus winIndex: " + winIndex);

            await GetWinMessage(winIndex);

            _logger.LogInformation("GetWinMessage winIndex: " + winIndex);
        }

        private void switchPassMoveId(int userId)
        {
            if (userId == 1)
            {
                passMovePlayerId = 2;
            }
            else if (userId == 2)
            {
                passMovePlayerId = 1;
            }
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            UsersOnline--;

            await Clients.All.SendAsync("GetOnlineUsers", UsersOnline, UsersAll);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetGameStatus(int winIndex)
        {
            await Clients.All.SendAsync("GameStatus", winIndex == 0);
        }

        public async Task GetWinMessage(int winIndex)
        {
            string winner = "";

            _logger.LogInformation("GetWinMessage (inside), winIndex = " + winIndex);

            switch (winIndex)
            {
                case 1:
                    winner = $"player: {Players[0].Username} won";
                    break;
                case 2:
                    winner = $"player: {Players[1].Username} won";
                    break;
                case 3:
                    winner = "DRAW";
                    break;
            }

            _logger.LogInformation($"GetWinMessage (inside) winIndex = {winIndex} | message = {winner}");

            await Clients.All.SendAsync("WinMessage", winner);

            _logger.LogInformation("message sended: " + winner);
        }

        public async Task GetCurrentPlayer()
        {
            await Clients.All.SendAsync("GetCurrentPlayer", passMovePlayerId);
        }

        public override async Task OnConnectedAsync()
        {
            UsersOnline++;
            UsersAll++;

            await SetUserId();

            await GetCurrentPlayer();

            await Clients.All.SendAsync("GetOnlineUsers", UsersOnline, UsersAll);

            await GetPassMove();

            await base.OnConnectedAsync();
        }


        public async Task SetCurrentPlayer(string inputId)
        {
            Guid newId = Guid.Parse(inputId);

            User currentUser = await _services.GetUserById(newId);

            Players.Add(new PlayerViewModel { Id = currentUser.Id, Username = currentUser.Username, PlayerId = PlayerIdCounter });

            await Clients.Caller.SendAsync("GetPlayerMessage", Players[Players.Count - 1].Username);

            _logger.LogInformation("player added");

            _logger.LogInformation($"player entered: Username: {Players[Players.Count - 1].Username} Guid-Id: {Players[Players.Count - 1].Id} PlayerId: {Players[Players.Count - 1].PlayerId}");

            PlayerIdCounter++;
        }
    }
}
