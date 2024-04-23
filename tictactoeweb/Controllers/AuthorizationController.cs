using Microsoft.AspNetCore.Mvc;
using tictactoeweb.Models;
using tictactoeweb.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using tictactoeweb.Models.MainModels;
using tictactoeweb.Context;
using tictactoeweb.Models.AuthorizationModels;
using Microsoft.EntityFrameworkCore;
using tictactoeweb.Services;

namespace tictactoeweb.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly ILogger<AuthorizationController> _logger;
        private UserDbContext _context;
        private UserServices _services;

        public AuthorizationController(ILogger<AuthorizationController> logger, UserDbContext context, UserServices services)
        {
            _logger = logger;
            _context = context;
            _services = services;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _services.GetRegisteredUser(model);

                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }
        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            

            if (ModelState.IsValid)
            {
                var regiseredUser = _services.GetUserByUsername(model.Username);

                if (regiseredUser != null)
                {
                    return View(model);
                }

                if (model.Password == model.PasswordConfirm)
                {
                    User user = new User
                    {
                        Username = model.Username,
                        Password = model.Password
                    };

                    Role userRole = await _services.GetRoleByName();

                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    await _services.AddUser(user);

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Authorization");
        }
    }
}
