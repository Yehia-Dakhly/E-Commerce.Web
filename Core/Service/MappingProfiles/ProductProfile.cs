using AutoMapper;
using DomainLayer.Models.ProductModule;
using Shared.DataTransferObjects.ProductDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTo>()
                .ForMember(P => P.ProductType, O => O.MapFrom(P => P.ProductBrand.Name))
                .ForMember(P => P.ProductBrand, O => O.MapFrom(P => P.ProductType.Name))
                .ForMember(P => P.PictureUrl, O => O.MapFrom<PictureUrlResolver>());

            CreateMap<ProductBrand, BrandDTo>();
            CreateMap<ProductType, TypeDTo>();
        }
    }
}
