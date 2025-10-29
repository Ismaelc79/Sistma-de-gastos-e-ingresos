using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class transactionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() {

            return Ok();
        }

        [HttpPost]
        public IActionResult Add()
        {

            return Created("","");
        }

    }
}
