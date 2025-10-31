using System.Net;
using Application.DTOs.Categories;
using Application.DTOs.Transactions;
using Application.Services;
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

        readonly CategoryService categoryService;


        [HttpGet]
        public IActionResult getCategories(){

            return Ok(categoryService.GetCategoriesByUserIdAsync);
           
        }

        [HttpPost]
        public IActionResult addCategorie(CreateCategoryRequest createCategory)
        {

            try
            {

              Task<CategoryDto> categoria = categoryService.CreateCategoryAsync(createCategory);
                return Created("", categoria);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex );
            }


        }

    }
}
