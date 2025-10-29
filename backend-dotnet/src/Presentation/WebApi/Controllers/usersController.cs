using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]/me")]
    public class usersController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get() {

            return Ok("Lista Users");
        }

        [HttpPatch]
        public IActionResult Patch() {
        

            return NoContent();
        }

    }
}
