using Bussiness.Interfaces;
using Data.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Bussiness.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace Client.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Appointment model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kullanıcının Id’sini al (cookie’den)
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Randevuya kullanıcıyı bağla
            model.UserID = userId;

            await _appointmentService.CreateAsync(model);
            return RedirectToAction("MyAppointments");
        }


        public async Task<IActionResult> MyAppointments()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("login", "Account");
            }
            var list = await _appointmentService.GetUserAppointmentAsync(userId.Value);
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(Appointment model)
        {
            Console.WriteLine("Edit POST çalıştı: " + model.Title);

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Alan: {state.Key}, Hata: {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            await _appointmentService.UpdateAsync(model);
            return RedirectToAction("MyAppointments");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return RedirectToAction("MyAppointments");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
