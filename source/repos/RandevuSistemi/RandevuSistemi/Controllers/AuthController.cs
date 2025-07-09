using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Business.Services;
using AppointmentSystem.Business;
using Microsoft.AspNetCore.Mvc;

namespace RandevuSistemi.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result)
                return BadRequest("Kullanıcı zaten mevcut.");

            return Ok("Kayıt başarılı.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Kullanıcı adı veya şifre yanlış.");

            return Ok(new { Token = token });
        }
    }

}
