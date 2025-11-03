using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]/me")]
    public class usersController : ControllerBase
    {
        private readonly IUserService userService;

        public usersController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            try
            {
                // Extracts user ID from the JW token
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { message = "No se encontró el ID del usuario en el token" });

                // Assigns UserId to request before send it to service
                var id = Ulid.Parse(userIdClaim);

                return Ok(await userService.GetUserByIdAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(401, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> Patch(UpdateProfileRequest upr) 
        {
            try
            {
                // Extracts user ID from the JW token
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { message = "No se encontró el ID del usuario en el token" });

                // Assigns UserId to request before send it to service
                var id = Ulid.Parse(userIdClaim);

                return Ok(await userService.UpdateUserProfileAsync(id, upr));
            }
            catch (Exception ex)
            {
                return StatusCode(401, new { message = ex.Message });
            }
        }
    }
}
