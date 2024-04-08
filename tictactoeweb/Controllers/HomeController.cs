using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using tictactoeweb.Context;
using tictactoeweb.Models;
using tictactoeweb.Models.HomeModels;
using tictactoeweb.Services;

namespace tictactoeweb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserServices _services;

        public HomeController(ILogger<HomeController> logger, UserDbContext context, UserServices services)
        {
            _logger = logger;
            _services = services;
        }

        public async Task<IActionResult> Index()
        {
            var userVM = new UserViewModel { Username = User.Identity.Name };

            var user = await _services.GetUserByUsername(userVM.Username);

            if (user != null)
            {
                userVM.Id = user.Id;
            }

            return View(userVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
