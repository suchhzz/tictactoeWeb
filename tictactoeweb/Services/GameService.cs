using tictactoeweb.Models.GameModels;

namespace tictactoeweb.Services
{
    public class GameService
    {
        public void SetPlayerMove(Room room, int cell)
        {
            room.Playground.Playfield[cell] = room.Playground.PassMoveId;

            if (checkWin(room))
            {
                room.Playground.WinIndex = room.Playground.PassMoveId;
            }

            room.Playground.MoveCounter++;

            switchPassMoveId(room);
        }

        private bool checkWin(Room room)
        {
            for (int i = 0; i < room.Playground.Playfield.Length; i += 3)
            {
                if (room.Playground.Playfield[i] != 0 && room.Playground.Playfield[i] == room.Playground.Playfield[i + 1] && room.Playground.Playfield[i] == room.Playground.Playfield[i + 2])
                    return true; 
            }

            for (int i = 0; i < 3; i++)
            {
                if (room.Playground.Playfield[i] != 0 && room.Playground.Playfield[i] == room.Playground.Playfield[i + 3] && room.Playground.Playfield[i] == room.Playground.Playfield[i + 6])
                    return true; 
            }

            if (room.Playground.Playfield[0] != 0 && room.Playground.Playfield[0] == room.Playground.Playfield[4] && room.Playground.Playfield[0] == room.Playground.Playfield[8])
                return true; 
            if (room.Playground.Playfield[2] != 0 && room.Playground.Playfield[2] == room.Playground.Playfield[4] && room.Playground.Playfield[2] == room.Playground.Playfield[6])
                return true; 

            return false; 
        }

        public string getWinMessage(Room room)
        {
            if (room.Playground.WinIndex == 3)
            {
                return "DRAW";
            }

            return $"{room.Players.Where(p => p.PlayerId == room.Playground.WinIndex).First().Username} WON";
        }

        private void switchPassMoveId(Room room)
        {
            if (room.Playground.PassMoveId == 1)
            {
                room.Playground.PassMoveId = 2;
            }
            else
            {
                room.Playground.PassMoveId = 1;
            }
        }
    }
}

