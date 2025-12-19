using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModuleDTos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = DomainLayer.Models.ProductModule.Product;
namespace Service
{
    public class PaymentService(IConfiguration _configuration, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDTo> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            // Configure Stripe - Install Package  Stripe.NET
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            // Get Basket By Basket ID
            var Basket = await _basketRepository.GetBasketAsync(BasketId) ?? throw new BasketNotFoundException(BasketId);
            // Get Amount - Get Products Price + Delivery Method Cost
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetbyIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = Product.Price;
            }
            ArgumentNullException.ThrowIfNull(Basket.DeliveryMethodId);
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetbyIdAsync(Basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFound(Basket.DeliveryMethodId.Value);
            Basket.ShippingPrice = DeliveryMethod.Cost;

            var BasketAmount = (long) (Basket.Items.Sum(I => I.Price * I.Quantity) + DeliveryMethod.Cost) * 100;
            // Create Payment Intent [Create | Update]
            var PaymentInService = new PaymentIntentService();
            if(Basket.PaymentIntentId is null) // Create
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = BasketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var PaymentIntent = await PaymentInService.CreateAsync(Options);
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else // Update
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = BasketAmount
                };
                await PaymentInService.UpdateAsync(Basket.PaymentIntentId, Options);
            }
            await _basketRepository.CreateOrUpdateBasketAsync(Basket);
            return _mapper.Map<BasketDTo>(Basket);
        }
    }
}
