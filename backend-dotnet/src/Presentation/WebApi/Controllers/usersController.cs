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

            return Ok(await userService.GetUserByIdAsync(id));
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(Ulid id, UpdateProfileRequest upr) {
        

            return Ok(await userService.UpdateUserProfileAsync(id, upr));
        }

    }
}
