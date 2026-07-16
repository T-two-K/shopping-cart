using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Contracts;
using ShoppingCart.Interfaces.IApp;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Models;
using System.Security.Claims;

namespace ShoppingCart.Controllers
{
    [AllowAnonymous]
    public class AccountController(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository) : Controller
    {
        private IPasswordHasher _passwordHasher = passwordHasher;
        private IUserRepository _userRepository = userRepository;

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            ViewBag.Title = "Регистрация";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel registerModel)
        {
            if (registerModel.Password != registerModel.RepeatPassword)
            {
                ModelState.AddModelError("", "Пароли не совпадают!");
                return View(registerModel);
            }

            if (!ModelState.IsValid)
                return View(registerModel);

            var existingUser = await _userRepository.GetByLoginAsync(registerModel.Login);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Пользователь под таким логином уже существует.");
                return View(registerModel);
            }

            var newUser = new User
            {
                Login = registerModel.Login,
                PasswordHash = _passwordHasher.CreateHash(registerModel.Password),
                Role = "user"
            };

            await _userRepository.AddAsync(newUser);

            return View("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Вход";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userRepository.GetByLoginAsync(model.Login);

            if (user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
                return View(model);
            }

            if (!_passwordHasher.CompareHashes(user.PasswordHash, model.Password))
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
                return View(model);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("MainPage", "Shop");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AccessDenied(string? param = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("MainPage", "Shop");
            }

            return View("Login");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
