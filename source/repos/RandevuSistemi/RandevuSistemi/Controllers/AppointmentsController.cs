using AppointmentSystem.Business.Services;
using AppointmentSystem.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RandevuSistemi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            await _service.AddAsync(appointment);
            return Ok();
        }
    }

}
