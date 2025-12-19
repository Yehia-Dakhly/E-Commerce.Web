using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects.ProductDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService(IUnitOfWork _unitofwork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDTo>> GetAllBrandsAsync()
        {
            var Brands = await _unitofwork.GetRepository<ProductBrand, int>().GetAllAsync();
            var BrandsDTos = _mapper.Map<IEnumerable<BrandDTo>>(Brands);
            return BrandsDTos;
        }

        public async Task<PaginatedResult<ProductDTo>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Specification = new ProductWithBrandAndTypeSpecification(queryParams);
            var Repo = _unitofwork.GetRepository<Product, int>();
            var Products = await Repo.GetAllAsync(Specification);
            var Data = _mapper.Map<IEnumerable<ProductDTo>>(Products);
            var CountSpec = new ProductCountSpecification(queryParams);
            var TotalSize = await Repo.CountAsync(CountSpec);
            return new PaginatedResult<ProductDTo>(queryParams.PageNumber, Data.Count(), TotalSize, Data);
        }

        public async Task<IEnumerable<TypeDTo>> GetAllTypesAsync()
        {
            var Types = await _unitofwork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTo>>(Types);
        }

        public async Task<ProductDTo> GetProductByIdAsync(int id)
        {
            var Specification = new ProductWithBrandAndTypeSpecification(id);
            var Product = await _unitofwork.GetRepository<Product, int>().GetbyIdAsync(Specification);
            if (Product is null)
            {
                throw new ProductNotFoundException(id);
            }
            return _mapper.Map<ProductDTo>(Product);
        }
    }
}
