using Application.DTOs.Auth;
using Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class authController : ControllerBase
    {

        readonly IAuthService authService;


        [HttpPost("/register")]
        public IActionResult Register(RegisterRequest registerRequest)
        {

            try
            {
                Task<AuthResponse> response = authService.RegisterAsync(registerRequest);
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
