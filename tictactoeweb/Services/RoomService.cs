using tictactoeweb.Models.GameModels;
using tictactoeweb.Models.HomeModels;
using tictactoeweb.Models.MainModels;

namespace tictactoeweb.Services
{
    public class RoomService
    {
        public List<Room> Rooms = new List<Room>();

        public async Task CreateRoom(List<UserViewModel> userList)
        {

            PlayerViewModel firstPlayer = new PlayerViewModel
            {
                Id = userList[0].Id,
                Username = userList[0].Username
            };

            PlayerViewModel secondPlayer = new PlayerViewModel
            {
                Id = userList[1].Id,
                Username = userList[1].Username
            };

            Room newRoom = new Room(firstPlayer, secondPlayer);

            Rooms.Add(newRoom);
        }

        public Room GetRoomById(Guid roomId)
        {
            return Rooms.FirstOrDefault(r => r.RoomId == roomId);
        }

        public void switchPassMoveId(Guid roomId, int userId)
        {
            Room currentRoom = Rooms.FirstOrDefault(r => r.RoomId == roomId);

            if (userId == 1)
            {
                currentRoom.PassMoveID = 2;
            }
            else if (userId == 2)
            {
                currentRoom.PassMoveID = 1;
            }
        }

        public async Task RemoveRoomById(Guid roomId)
        {
            Rooms.Remove(GetRoomById(roomId));
        }
    }
}
