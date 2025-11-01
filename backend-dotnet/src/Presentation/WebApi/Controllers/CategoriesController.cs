﻿using System.Net;
using Application.DTOs.Categories;
using Application.DTOs.Transactions;
using Application.Interfaces;
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

        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategoriesByUserId(Ulid userID){

            var categorias = await _categoryService.GetCategoriesByUserIdAsync(userID); 
            return Ok(categorias);
            
        }

        [HttpPost]
        public IActionResult Add(CreateCategoryRequest createCategory)
        {

            try
            {

              Task<CategoryDto> categoria = _categoryService.CreateCategoryAsync(createCategory);
                return Created("", categoria);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex );
            }


        }

    }
}
