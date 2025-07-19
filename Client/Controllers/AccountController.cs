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
                return RedirectToAction(nameof(Login));

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

                return RedirectToAction(nameof(UserPanel));
            }

            ModelState.AddModelError("", "Hatalı Email veya şifre.");
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> SelectAppointment(DateTime? date)
        {

            var targetDate = DateTime.Today.AddDays(1);
            var appointments = await _userService.GetAvailableAppointmentsAsync(targetDate);

            
            ViewBag.CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine($"Target date: {targetDate}");
            Console.WriteLine($"Found appointments: {appointments.Count}");
            return View(appointments);


        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(int appointmentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return RedirectToAction("Login");
            }

            var userId = int.Parse(userIdClaim);
            var result = await _userService.BookAppointmentAsync(userId, appointmentId);

            if (result == null)
            {
                TempData["Error"] = "Randevu alınamadı.";
            }
            else
            {
                TempData["Success"] = "Randevu başarıyla alındı.";
            }

            return RedirectToAction("SelectAppointment");
        }


        //Girş ekranına atma

        public IActionResult UserPanel()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            ViewBag.UserFullName = HttpContext.Session.GetString("UserFullName");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Tüm session bilgilerini siler
            return RedirectToAction(nameof(Login));

        }




    }


}

