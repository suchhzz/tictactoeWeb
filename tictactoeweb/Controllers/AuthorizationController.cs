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

namespace tictactoeweb.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly ILogger<AuthorizationController> _logger;
        private UserDbContext _context;

        public AuthorizationController(ILogger<AuthorizationController> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
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
                var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

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
                if (model.Password == model.PasswordConfirm)
                {
                    User user = new User
                    {
                        Username = model.Username,
                        Password = model.Password
                    };

                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");

                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim> // список из утверждений пользователя (в данном примере только один - userName)
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username), // создается обьект Claim с типом значения DefaultNameClaimType и значением userName
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name) // куки теперь имеет информацию о роли пользователя
            };
            // создаем объект ClaimsIdentity структурирует выше введенные Claim в удобный вид, можно будет обратиться к пользователю через User.Identity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки // браузер заносит информацию в куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Authorization");
        }
    }
}
