using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tictactoeweb.Context;
using tictactoeweb.Models.GameModels;
using tictactoeweb.Services;

namespace tictactoeweb.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private UserServices _services;
        private readonly RoomService _roomService;

        public GameController(UserServices services, RoomService roomService)
        {
            _services = services;
            _roomService = roomService;
        }


        public async Task<IActionResult> StartGame(string sendRoomId)
        {
            var room = _roomService.GetRoomById(Guid.Parse(sendRoomId));

            var gameViewModel = new GameViewModel
            {
                currentRoom = room,
                CurrentUser = await _services.GetUserByUsername(User.Identity.Name)
            };

            return View(gameViewModel);
        }

        public async Task<IActionResult> FindGame()
        {
            var user = await _services.GetUserByUsername(User.Identity.Name);

            return View(user);
        }
    }
}
