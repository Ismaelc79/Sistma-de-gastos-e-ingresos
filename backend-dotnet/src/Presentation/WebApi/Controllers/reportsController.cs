using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class reportsController : ControllerBase
    {
        [HttpGet("/summary")]
        public IActionResult Get() {

            return Ok("");
        }
    }
}
