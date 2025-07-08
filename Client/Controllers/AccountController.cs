using Bussiness.Interfaces;
using Client.Models;
using Data.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Bussiness.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Client.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result = await _userService.RegisterAsync(user, model.Password);
            if (result)
                return RedirectToAction("Login");

            ModelState.AddModelError("", "Kayıt başarısız, email zaten kayıtlı.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userService.LoginAsync(email, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                };
                var identify=new ClaimsIdentity(claims,"MyCookieAuth");
                var principal=new ClaimsPrincipal(identify);
                await HttpContext.SignInAsync("MyCookieAuth", principal);

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserFullName", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetString("UserRole", user.Role);

                return RedirectToAction("Panel");
            }

            ModelState.AddModelError("", "Hatalı Email veya şifre.");
            return View();
        }


        public IActionResult Panel()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.UserFullName = HttpContext.Session.GetString("UserFullName");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Tüm session bilgilerini siler
            return RedirectToAction("Login");
        }
      



    }


}

