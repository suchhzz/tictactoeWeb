using Microsoft.AspNetCore.Mvc;

namespace tictactoeweb.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Game()
        {
            return View();
        }

        public IActionResult FindGame()
        {
            return View();
        }
    }
}
