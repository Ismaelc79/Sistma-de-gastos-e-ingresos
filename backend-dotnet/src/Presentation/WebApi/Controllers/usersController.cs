using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> Get(Ulid id) 
        {
            try
            {
                return Ok(await userService.GetUserByIdAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(401, new { message = ex.Message });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(Ulid id, UpdateProfileRequest upr) 
        {
            try
            {
                return Ok(await userService.UpdateUserProfileAsync(id, upr));
            }
            catch (Exception ex)
            {
                return StatusCode(401, new { message = ex.Message });
            }
        }
    }
}
