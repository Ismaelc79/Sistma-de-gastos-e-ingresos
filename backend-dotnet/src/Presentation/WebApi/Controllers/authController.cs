using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly IAuthService authService;

        public authController(IAuthService authService)
        {
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                AuthResponse response = await authService.RegisterAsync(registerRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(401, new { message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                AuthResponse authResponse = await authService.LoginAsync(loginRequest);
                return Ok(authResponse);
            }
            catch (Exception ex) {
                return StatusCode(401, new { message = ex.Message});
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                AuthResponse authResponse = await authService.RefreshTokenAsync(refreshTokenRequest);
                return Ok(refreshTokenRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(401, new { message = ex.Message });
            }
        }

        [HttpPost("phone/start")]
        public IActionResult phoneStart()
        {
            return Ok();
        }

        [HttpPost("phone/verify")]
        public IActionResult phoneVerify()
        {
            return Ok();
        }



    }
}
