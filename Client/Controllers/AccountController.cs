using Bussiness.Interfaces;
using Client.Models;
using Data.Entitys;
using Microsoft.AspNetCore.Mvc;

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
                
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Hatalı Email veya şifre.");
            return View();
        }
    }

}

