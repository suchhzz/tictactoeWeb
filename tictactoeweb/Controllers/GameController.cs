using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tictactoeweb.Context;
using tictactoeweb.Models.GameModels;
using tictactoeweb.Models.HomeModels;
using tictactoeweb.Services;

namespace tictactoeweb.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private UserServices _services;

        public GameController(UserServices services)
        {
            _services = services;
        }

        public async Task<IActionResult> StartGame(string gameRoom)
        {
            UserViewModel player = new UserViewModel { Username = User.Identity.Name };

            var user = await _services.GetUserByUsername(player.Username);

            if (user != null)
            {
                player.Id = user.Id;
            }

            return View(player);
        }

        public async Task<IActionResult> FindGame()
        {
            UserViewModel player = new UserViewModel { Username = User.Identity.Name };

            var user = await _services.GetUserByUsername(player.Username);

            if (user != null)
            {
                player.Id = user.Id;
            }

            return View(player);
        }
    }
}
