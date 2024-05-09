using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using tictactoeweb.Context;
using tictactoeweb.Models;
using tictactoeweb.Models.AuthorizationModels;
using tictactoeweb.Models.HomeModels;
using tictactoeweb.Models.MainModels;
using tictactoeweb.Services;

namespace tictactoeweb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserService _services;

        public HomeController(ILogger<HomeController> logger, UserDbContext context, UserService services)
        {
            _logger = logger;
            _services = services;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _services.GetUserByUsername(User.Identity.Name);

            return View(user);
        }

        [Authorize(Roles="admin")]
        public async Task<IActionResult> AdminPanel()
        {
            var adminVM = new AdminPanelViewModel
            {
                UserList = await _services.GetAllUsers(),
                AdminCreate = new Models.AuthorizationModels.RegisterModel()
            };

            return View(adminVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _services.DeleteUserById(id);

            return RedirectToAction(nameof(AdminPanel));
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _services.GetUserById(Guid.Parse(id));

            return View(user);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> EditUser(User modifiedUser)
        {
            await _services.UpdateUser(modifiedUser);

            return RedirectToAction(nameof(AdminPanel));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminPanelViewModel adminVM)
        {
            var registeredAdmin = adminVM.AdminCreate;

                var alreadyRegistered = await _services.GetUserByUsername(registeredAdmin.Username);

                if (alreadyRegistered == null && registeredAdmin.Password == registeredAdmin.PasswordConfirm)
                {
                    var newAdmin = new User
                    {
                        Username = registeredAdmin.Username,
                        Password = registeredAdmin.Password,
                        Role = await _services.GetRoleByName("admin"),
                        Games = 0,
                        Wins = 0
                    };
                    await _services.AddUser(newAdmin);
                }
            

            return RedirectToAction(nameof(AdminPanel));
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
