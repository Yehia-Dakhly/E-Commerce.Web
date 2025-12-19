using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModuleDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BasketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDTo> CreateOrUpdateBasketAsync(BasketDTo basket)
        {
            var CustBasket = _mapper.Map<BasketDTo, CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(CustBasket);
            if (CreatedOrUpdatedBasket != null)
            {
                return await GetBasketAsync(basket.Id);
            }
            else
            {
                throw new Exception("Can Not Update Or Create Basket Now!\n Tyr Again Later.");
            }
        }
        public async Task<bool> DeleteBasketAsync(string Key)
                => await _basketRepository.DeleteBasketAsync(Key);


        public async Task<BasketDTo> GetBasketAsync(string Key)
        {
            var Basket = await _basketRepository.GetBasketAsync(Key);
            if (Basket is not null)
            {
                var BasketDTo = _mapper.Map<BasketDTo>(Basket);
                return BasketDTo;
            }
            else
            {
                throw new BasketNotFoundException(Key);
            }
        }
    }
}
