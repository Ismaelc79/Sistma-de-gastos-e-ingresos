using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Application.DTOs.Categories;
using Application.DTOs.Transactions;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]


    public class categoriesController: ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public categoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCategoriesByUserId()
        {
            // Extracts user ID from the JW token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { message = "No se encontró el ID del usuario en el token" });

            var userId = Ulid.Parse(userIdClaim);

            var categorias = await _categoryService.GetCategoriesByUserIdAsync(userId); 
            return Ok(categorias);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCategoryRequest createCategory)
        {
            try
            {
                // Extracts user ID from the JW token
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { message = "No se encontró el ID del usuario en el token"});

                // Assigns UserId to request before send it to service
                createCategory.UserId = Ulid.Parse(userIdClaim);

                CategoryDto categoria = await _categoryService.CreateCategoryAsync(createCategory);
                return Created("", categoria);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { message = ex.Message});
            }
        }
    }
}
