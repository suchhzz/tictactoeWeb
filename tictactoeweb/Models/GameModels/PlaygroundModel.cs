namespace tictactoeweb.Models.GameModels
{
    public class PlaygroundModel
    {
        public int MoveCounter { get; set; } = 0;
        public int PassMoveId { get; set; } = 1;
        public int WinIndex { get; set; } = 0;
        public int SetPlayerId { get; set; } = 1;
        public int[] Playfield { get; set; } = new int[9];
    }
}
