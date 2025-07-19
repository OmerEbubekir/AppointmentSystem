using Bussiness.Interfaces;
using Data.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User model)
        {
            
            if (!ModelState.IsValid)
            {
               
                return View(model);
            }
               
            
            await _adminService.AddUserAsync(model);
            return RedirectToAction(nameof(Panel));
        }
        [HttpGet]
        public IActionResult AddAppointment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(Appointment model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _adminService.AddAppointmentAsync(model);
            return RedirectToAction(nameof(GetAllAppointment));
        }

        [HttpGet]
        public IActionResult DeleteUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var success = await _adminService.DeleteUserAsync(userId);
            if (!success)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı veya silinemedi.");
                return View();
            }
            return RedirectToAction(nameof(Panel));
        }

        [HttpGet]
        public IActionResult DeleteAppointment()
        {
            return RedirectToAction(nameof(Panel));


        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
            var success = await _adminService.DeleteAppointmentAsync(appointmentId);
            if (!success)
            {
                ModelState.AddModelError("", "Randevu bulunamadı veya silinemedi.");
            }

            return RedirectToAction(nameof(GetAllAppointment));
        }


        [HttpGet]
        public IActionResult EditAppointment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditAppointment(Appointment appointment)
        {
            var result = await _adminService.EditAppointmentAsync(appointment);
            if (!result)
            {
                ModelState.AddModelError("", "Randevu bulunamadı veya güncellenemedi.");
                return View(appointment);
            }
            return RedirectToAction(nameof(Panel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointment()
        {
            var appointments = await _adminService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            var user = await _adminService.GetUserDetailsAsync(userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(User user)
        {
            try
            {
                await _adminService.UpdateUserRoleAsync(user);
                return RedirectToAction(nameof(Panel));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }
        public IActionResult Index()
        {


            return RedirectToAction(nameof(Panel));
        }

        public async Task<IActionResult> Panel()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            var user =await _adminService.GetAllUsersAsync();
            return View(user);
        }
    }
}
