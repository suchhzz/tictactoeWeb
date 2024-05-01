namespace tictactoeweb.Models.GameModels
{
    public class Room
    {
        public Guid RoomId { get; set; }
        public List<Player> Players { get; set; }
        public PlaygroundModel Playground { get; set; }
    }
}
