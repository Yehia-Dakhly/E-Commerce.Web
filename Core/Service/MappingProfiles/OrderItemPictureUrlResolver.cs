using AutoMapper;
using DomainLayer.Models.OrderModule;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects.OrderModuleDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    internal class OrderItemPictureUrlResolver(IConfiguration Configuration) : IValueResolver<OrderItem, OrderItemDTo, string>
    {
        public string Resolve(OrderItem source, OrderItemDTo destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl))
                return string.Empty;
            var Url = $"{Configuration.GetSection("Urls")["BaseUrl"]}{source.Product.PictureUrl}";
            return Url;
        }
    }
}
