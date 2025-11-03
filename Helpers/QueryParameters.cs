using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_web_api.Helpers
{
    public class QueryParameters
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public string? SortOrder { get; set; }

        public QueryParameters Validate()
        {
            if (PageSize > MaxPageSize)
            {
                PageSize = MaxPageSize;
            }
            if (PageNumber <= 0)
            {
                PageNumber = 1;
            }
            if (PageSize <= 0)
            {
                PageSize = 10;
            }
            return this;
        }
    }
}