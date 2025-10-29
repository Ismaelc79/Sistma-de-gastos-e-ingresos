using System.Net;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
     public class CategoriesController: ControllerBase
    {

            
        [HttpGet]
        public IActionResult getCategories(){

            return Ok();
        }

        [HttpPost]
        public IActionResult addCategorie()
        {

            return Created("","");
        }

    }
}
