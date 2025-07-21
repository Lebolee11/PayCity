// payCityUtilitiesApp.Api/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using payCityUtilitiesApp.Api.DTOs;
using payCityUtilitiesApp.Api.Services.Interfaces;
using System.Threading.Tasks; // For Task

namespace PayCityAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.LoginAsync(request);
            if (response.Token == null)
            {
                return Unauthorized(new { message = response.Message });
            }
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authService.ForgotPasswordAsync(request);
            return Ok(new { message = "If your email exists in our system, a password reset link has been sent." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterAsync(request);
            if (response.UserId == Guid.Empty)
            {
                return BadRequest(new { message = response.Message });
            }
            return StatusCode(201, response); // 201 Created
        }
    }
}