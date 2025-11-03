using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Helpers;


namespace Ecommerce_web_api.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedResult<ReadCategoryDto>> GetAllCategories(QueryParameters queryParameters);

        Task<ReadCategoryDto> CreateCategory(CreateCategoryDto categoryData);

        Task<ReadCategoryDto?> GetCategoryById(Guid categoryId);

        Task<bool> DeleteCategoryById(Guid categoryId);

        Task<ReadCategoryDto?> UpdateCategoryById(Guid categoryId,UpdateCategoryDto categoryData);
    }
}