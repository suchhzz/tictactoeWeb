using tictactoeweb.Models.GameModels;
using tictactoeweb.Models.MainModels;

namespace tictactoeweb.Services
{
    public class RoomService
    {
        public List<Room> Rooms = new List<Room>();

        public async Task CreateRoom(List<User> userList)
        {
            Room newRoom = new Room
            {
                RoomId = Guid.NewGuid(),

                Players = new List<Player>
                {
                    new Player { PlayerId = 1, Id = userList[0].Id, Username = userList[0].Username },
                    new Player { PlayerId = 2, Id = userList[1].Id, Username = userList[1].Username }
                },

                Playground = new PlaygroundModel()
            };

            Rooms.Add(newRoom);
        }

        public Room GetRoomById(Guid roomId)
        {
            return Rooms.FirstOrDefault(r => r.RoomId == roomId);
        }

        public async Task RemoveRoomById(Guid roomId)
        {
            Rooms.Remove(GetRoomById(roomId));
        }
    }
}
