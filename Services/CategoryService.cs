using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Interfaces;
using AutoMapper;
using Ecommerce_web_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Tracing;
using Ecommerce_web_api.Enums;
using Ecommerce_web_api.Helpers;

namespace Ecommerce_web_api.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext appDbContext,IMapper mapper)
        {
            _appDbContext=appDbContext;
            _mapper=mapper;
        }

        public async Task<PaginatedResult<ReadCategoryDto>> GetAllCategories
        (QueryParameters queryParameters)
        {
            var pageNumber=queryParameters.PageNumber;
            var pageSize=queryParameters.PageSize;
            var search=queryParameters.Search;
            var sortOrder=queryParameters.SortOrder;
        
            IQueryable<Category> query = _appDbContext.Categories;
            if (!string.IsNullOrEmpty(search))
            {
                var formatedSearch = $"%{search.Trim()}%";
                query = query.Where(c => EF.Functions.ILike(c.Name, formatedSearch) ||
                                         EF.Functions.ILike(c.Description, formatedSearch));
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                query = query.OrderBy(c => c.Name);
            }
            else
            {
                var formattedSortOrder = sortOrder.Trim().ToLower();
                if(Enum.TryParse<SortOrder>(formattedSortOrder, true, out var parsedSortOrder))
                {
                    query = parsedSortOrder switch
                    {
                        SortOrder.NameAsc => query.OrderBy(c => c.Name),
                        SortOrder.NameDesc => query.OrderByDescending(c => c.Name),
                        SortOrder.CreatedAtAsc => query.OrderBy(c => c.CreatedAt),
                        SortOrder.CreatedAtDesc => query.OrderByDescending(c => c.CreatedAt),
                        _ => query.OrderBy(c => c.Name)
                    };
                }
            }

                var totalCount = await query.CountAsync();

            var items = await query.Skip((pageNumber - 1) * pageSize)

            .Take(pageSize).ToListAsync();

            var result = _mapper.Map<List<ReadCategoryDto>>(items);
            return new PaginatedResult<ReadCategoryDto>
            {
                Items = result,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }

        public async Task<ReadCategoryDto> CreateCategory(CreateCategoryDto categoryData)
        {
           var newCategory=_mapper.Map<Category>(categoryData);
           await _appDbContext.Categories.AddAsync(newCategory);
           await _appDbContext.SaveChangesAsync();
           return _mapper.Map<ReadCategoryDto>(newCategory);    
        }

        public async Task<ReadCategoryDto?> GetCategoryById(Guid categoryId)
        {
            var foundCategory=await _appDbContext.Categories.FindAsync(categoryId);
            return foundCategory == null ? null: _mapper
            .Map<ReadCategoryDto>(foundCategory);
        }

        public async Task<ReadCategoryDto?> UpdateCategoryById(Guid categoryId,UpdateCategoryDto categoryData)
        {
            var foundCategory=await _appDbContext.Categories.FindAsync(categoryId);
            if(foundCategory==null)
            {
                return null;
            }
            _mapper.Map(categoryData, foundCategory);
            _appDbContext.Categories.Update(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<ReadCategoryDto>(foundCategory);
        }

        public async Task<bool> DeleteCategoryById(Guid categoryId)
        {
            var foundCategory=await _appDbContext.Categories.FindAsync(categoryId);
            if(foundCategory==null)
            {
                return false;
            }
            _appDbContext.Categories.Remove(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return true;
            
        }
    }
}