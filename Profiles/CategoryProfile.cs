using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Models;

namespace Ecommerce_web_api.Profiles
{
    public class CategoryProfile :Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryDto,Category>();
            CreateMap<Category,ReadCategoryDto>();
            CreateMap<UpdateCategoryDto,Category>();
        }
    }
}