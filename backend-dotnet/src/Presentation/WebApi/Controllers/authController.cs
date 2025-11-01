using Application.DTOs.Auth;
using Application.Interfaces;

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

        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {

            try
            {
                AuthResponse response = await authService.RegisterAsync(registerRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(401, ex);
            }

        }

        [HttpPost("/login")]
        public IActionResult login(LoginRequest loginRequest)
        {

            try
            {
                Task<AuthResponse> authResponse = authService.LoginAsync(loginRequest);
                return Ok(authResponse);
            }
            catch (Exception ex) {
                return StatusCode(401, ex);
            }
        
            
        }

        [HttpPost("/refresh")]
        public IActionResult Refresh(RefreshTokenRequest refreshTokenRequest)
        {


            try
            {

                Task<AuthResponse> authResponse = authService.RefreshTokenAsync(refreshTokenRequest);
                return Ok(refreshTokenRequest);

            }
            catch (Exception ex)
            {
                return StatusCode(401, ex);
            }
            
        }

        [HttpPost("/phone/start")]
        public IActionResult phoneStart()
        {
            return Ok();
        }

        [HttpPost("/phone/verify")]
        public IActionResult phoneVerify()
        {
            return Ok();
        }



    }
}
