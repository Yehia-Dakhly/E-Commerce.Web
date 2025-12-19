using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductQueryParams
    {
        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public ProductSortingOptions Sort { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;

        private int pageSize = DefaultPageSize;
        public int PageSize { 
            get { return pageSize; }
            set {  pageSize = value > MaxPageSize ? MaxPageSize : value; } 
        }
    }
}
