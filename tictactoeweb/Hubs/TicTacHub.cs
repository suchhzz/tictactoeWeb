using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace tictactoeweb.Hubs
{
    public class TicTacHub : Hub
    {
        public static int Id { get; set; } = 1;
        public static int UsersOnline { get; set; } = 0;
        public static int UsersAll { get; set; } = 0;
        public static int MoveCounter { get; set; } = 0;
        public async Task SetUserId()
        {
            await Clients.Caller.SendAsync("GetUserId", Id);
            Id++;
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

                await GameOver(winIndex);
            }
            else
            {
                switchPassMoveId(userId);
                await GetPassMove();
            }

            await Clients.All.SendAsync("ReceiveMove", userId, cell, winIndex);

            

        }

        public async Task GameOver(int winIndex)
        {
            await GetGameStatus(winIndex);
            await GetWinMessage(winIndex);
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

            switch (winIndex)
            {
                case 1:
                    winner = "first player WON";
                    break;
                case 2:
                    winner = "second player WON";
                    break;
                case 3:
                    winner = "DRAW";
                    break;
            }

            await Clients.All.SendAsync("WinMessage", winner);
        }

        public async Task GetCurrentPlayer()
        {
            await Clients.All.SendAsync("GetCurrentPlayer", passMovePlayerId);
        }

        public override async Task OnConnectedAsync()
        {
            UsersOnline++;
            UsersAll++;

            await GetCurrentPlayer();

            await Clients.All.SendAsync("GetOnlineUsers", UsersOnline, UsersAll);

            await GetPassMove();

            await SetUserId();

            await base.OnConnectedAsync();
        }

    }
}
