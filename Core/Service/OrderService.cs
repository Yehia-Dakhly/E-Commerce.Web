using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransferObjects.OrderModuleDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class OrderService(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDTo> CreateOrderAsync(OrderDTo orderDTo, string Email)
        {
            // Map AddressDTo To Address
            var OrderAddress = _mapper.Map<OrderAddress>(orderDTo.ShipToAddress);
            // Get Basket -> Create The OrderItems List
            var Basket = await _basketRepository.GetBasketAsync(orderDTo.BasketId) ?? throw new BasketNotFoundException(orderDTo.BasketId);
            ArgumentNullException.ThrowIfNullOrEmpty(Basket.PaymentIntentId);

            var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var Specification = new OrderWithPaymentIntentIdSpecification(Basket.PaymentIntentId);
            var ExistingOrder = await OrderRepo.GetbyIdAsync(Specification);
            if (ExistingOrder is not null)
            {
                OrderRepo.Delete(ExistingOrder);
            }
            List<OrderItem> orderItems = [];
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetbyIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                var OrderItem = new OrderItem()
                {
                    Product = new ProductItemOrdered() { ProductId = item.Id, PictureUrl = item.PictureUrl, ProductName = item.ProductName },
                    Price = Product.Price,
                    Quantity = item.Quantity,
                };
                orderItems.Add(OrderItem);
            }

            // Get Delivery Method
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetbyIdAsync(orderDTo.DeliveryMethodId) ?? throw new DeliveryMethodNotFound(orderDTo.DeliveryMethodId);
            // Calculate SubTotal
            var SubTotal = orderItems.Sum(I => I.Price *  I.Quantity);
            var Order = new Order(Email, OrderAddress, DeliveryMethod, orderItems, SubTotal, Basket.PaymentIntentId);

            await OrderRepo.AddAsync(Order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderToReturnDTo>(Order);
        }

        public async Task<IEnumerable<DeliveryMethodDTo>> GetAllDeliveryMethodsAsync()
        {
            var DeliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            //if (!DeliveryMethods.Any())
            //{
            //    //_logger.LogWarning("No delivery methods configured in the database.");
            //}
            return _mapper.Map<IEnumerable<DeliveryMethodDTo>>(DeliveryMethods);
        }

        public async Task<IEnumerable<OrderToReturnDTo>> GetAllOrdersByEmailAsync(string Email)
        {
            var Specification = new OrdersSpecifications(Email);
            var Orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(Specification);
            return _mapper.Map<IEnumerable<OrderToReturnDTo>>(Orders);
        }

        public async Task<OrderToReturnDTo> GetOrderByIdAsync(Guid id)
        {
            var Specification = new OrdersSpecifications(id);
            var Order = await _unitOfWork.GetRepository<Order, Guid>().GetbyIdAsync(Specification) ?? throw new NotFoundOrderException(id);
            return _mapper.Map<OrderToReturnDTo>(Order);
        }
    }
}
