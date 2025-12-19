using AutoMapper;
using DomainLayer.Models.OrderModule;
using Shared.DataTransferObjects.IdentityDTos;
using Shared.DataTransferObjects.OrderModuleDTos;


namespace Service.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTo, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDTo>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName));
            CreateMap<OrderItem, OrderItemDTo>()
                .ForMember(D => D.ProductName, O => O.MapFrom(D => D.Product.ProductName))
                .ForMember(D => D.Picture, O => O.MapFrom<OrderItemPictureUrlResolver>());
            CreateMap<DeliveryMethod, DeliveryMethodDTo>();
        }
    }
}
