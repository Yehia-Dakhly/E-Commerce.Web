using Shared;
using Shared.DataTransferObjects.ProductDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductDTo>> GetAllProductsAsync(ProductQueryParams queryParams);
        Task<ProductDTo> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDTo>> GetAllBrandsAsync();
        Task<IEnumerable<TypeDTo>> GetAllTypesAsync();
    }
}
