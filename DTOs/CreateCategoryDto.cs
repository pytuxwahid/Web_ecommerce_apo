using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_web_api.DTOs
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage ="Category name is required")]
        [StringLength(100,MinimumLength=2,ErrorMessage ="Category name must be between 2 and 100 characters long")]
        public string Name{get;set;}=string.Empty;
        [StringLength(500,ErrorMessage ="Description can't exceed 500 characters")]
        public string Description{get;set;}=string.Empty;
    }
}