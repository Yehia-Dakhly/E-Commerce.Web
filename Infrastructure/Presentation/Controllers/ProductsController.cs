using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects.ProductDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
     // BaseUrl/api/Products
    public class ProductsController(IServiceManager serviceManager) : ApiBaseController
    {
        //[Authorize(Roles ="Admin")]////////////

        // GetAllProducts
        [HttpGet]
        // GET BaseUrl/api/Products
        [Cache]
        public async Task<ActionResult<IEnumerable<ProductDTo>>>GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
            return Ok(await serviceManager.ProductService.GetAllProductsAsync(queryParams));
        }
        // GetProductbyId
        [HttpGet("{id:int}")]
        // GET BaseUrl/api/Products/10
        public async Task<ActionResult<ProductDTo>> GetProduct(int id)
        {
            return Ok(await serviceManager.ProductService.GetProductByIdAsync(id));
        }
        // GetAllTypes
        [HttpGet("types")]
        // GET BaseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeDTo>>> GetTypes()
        {
            return Ok(await serviceManager.ProductService.GetAllTypesAsync());
        }
        // GetAllBrands
        [HttpGet("brands")]
        // GET BaseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDTo>>> GetBrands()
        {
            return Ok(await serviceManager.ProductService.GetAllBrandsAsync());
        }
        
    }
}
