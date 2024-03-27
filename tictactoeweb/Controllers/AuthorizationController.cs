using Microsoft.AspNetCore.Mvc;
using tictactoeweb.Migrations;
using tictactoeweb.Models;

namespace tictactoeweb.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(ILogger<AuthorizationController> logger)
        {
            _logger = logger;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(AuthorizationViewModel? authorizationVM)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User();

                newUser.Username = authorizationVM.Username;
                newUser.Password = authorizationVM.Password;


                if (newUser != null)
                {
                    return RedirectToAction(nameof(Success), newUser);
                }
            }

            return View();
        }
        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(AuthorizationViewModel? authorizationVM)
        {
            if (ModelState.IsValid)
            {
                if (authorizationVM.Password == authorizationVM.PasswordConfirm)
                {
                    User newUser = new User();

                    newUser.Username = authorizationVM.Username;
                    newUser.Password = authorizationVM.Password;

                    return RedirectToAction(nameof(Success), newUser);

                }
            }

            return View();
        }
        public IActionResult Success(User user)
        {
            return View(user);
        }
    }
}
