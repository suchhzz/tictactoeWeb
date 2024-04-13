namespace tictactoeweb.Models.GameModels
{
    public class Room
    {
        public Room(PlayerViewModel player1, PlayerViewModel player2)
        {
            player1.PlayerId = 1;
            player2.PlayerId = 2;

            Players.Add(player1);
            Players.Add(player2);

            RoomId = Guid.NewGuid();
        }
        public Guid RoomId { get; set; }
        public List<PlayerViewModel> Players = new List<PlayerViewModel>();
        public int PassMoveID { get; set; } = 1;
        public int WinIndex { get; set; } = 0;
        public int MoveCounter { get; set; } = 0;
        public int SetPlayerId { get; set; } = 1;
    }
}
