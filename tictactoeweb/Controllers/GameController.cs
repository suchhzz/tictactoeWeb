using Microsoft.AspNetCore.Mvc;

namespace tictactoeweb.Controllers
{
    public class GameController : Controller
    {
        public IActionResult FindGame()
        {
            return View();
        }
    }
}
