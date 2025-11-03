using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_web_api.Models;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Services;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Helpers;


namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("v1/api/categories/")]

    public class CategoryController :ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService=categoryService;
        }

        //Get: api/categories=>read categories
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] QueryParameters queryParameters)
        {
            queryParameters.Validate();
            var categoryList=await _categoryService.GetAllCategories(queryParameters);
            return Ok(ApiResponse<PaginatedResult<ReadCategoryDto>>.SuccessResponse
            (categoryList,200,"Categories return successfully"));
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        { 
            var category=await _categoryService.GetCategoryById(categoryId);
            if (category == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse
                (new List<string>{"Category with this id not exist"},404,"Validation Failed"));
            }
            
            return Ok(ApiResponse<ReadCategoryDto>.SuccessResponse
            (category,200,"Category is  return successfully"));
        }
    

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryData)
        {
            var categoryReadDto=await _categoryService.CreateCategory(categoryData);
                
            return Created(nameof(GetCategoryById),
            ApiResponse<ReadCategoryDto>.SuccessResponse
            (categoryReadDto,201,"Category created successfully"));
        }

        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult> DeleteCategoryById(Guid categoryId)
        {
            var foundCategory=await _categoryService.DeleteCategoryById(categoryId);
            if (!foundCategory)
            {
                return NotFound(ApiResponse<object>.ErrorResponse
                (new List<string>{"Category with this id not exist"},404,"Validation Failed"));
            }
            
            return Ok(ApiResponse<object>.SuccessResponse
            (null,200,"Category deleted successfully"));
        }
        
        [HttpPut("{categoryId:guid}")]
        public async Task<IActionResult> UpdateCategoryById(Guid categoryId,[FromBody] UpdateCategoryDto categoryData)
        {
            var updateCategory=await _categoryService.UpdateCategoryById(categoryId,categoryData);
            if (updateCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse
                (new List<string>{"Category with this id not exist"},404,"Validation Failed"));
            }
           
            
            return Ok(ApiResponse<ReadCategoryDto>.SuccessResponse
            (updateCategory,204,"Category updated successfully"));
        }
    }
}