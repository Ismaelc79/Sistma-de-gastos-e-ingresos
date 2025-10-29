using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class authController : ControllerBase
    {

        [HttpPost("/register")]
        public IActionResult register()
        {
            return Created("", "");
        }

        [HttpPost("/login")]
        public IActionResult login()
        {
            return Ok();
        }

        [HttpPost("/phone/refresh")]
        public IActionResult phoneRefresh()
        {
            return Ok();
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
