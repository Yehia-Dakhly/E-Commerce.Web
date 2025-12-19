using AutoMapper;
using DomainLayer.Models.IdentityModule;
using Shared.DataTransferObjects.IdentityDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    internal class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<RegisterDTo, ApplicationUser>().ReverseMap();
            CreateMap<UserDTo, ApplicationUser>().ReverseMap();
            CreateMap<Address, AddressDTo>().ReverseMap();
        }
    }
}
