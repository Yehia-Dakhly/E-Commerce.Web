using Shared.DataTransferObjects.OrderModuleDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IOrderService
    {
        // Create Order
        // Take [BasketId, Shipping Address, Dilvery Method Id], Customer Email
        // Return Order Details
        // (Id, UserEmail, OrderDate, Items (ProductName - Picture Url - Price - Quantity),
        // Address, Delivery Method Name, Order Status Value, SubTotal, Total Price
        Task<OrderToReturnDTo> CreateOrderAsync(OrderDTo orderDTo, string Email);
        Task<IEnumerable<DeliveryMethodDTo>> GetAllDeliveryMethodsAsync();
        Task<IEnumerable<OrderToReturnDTo>> GetAllOrdersByEmailAsync(string Email);
        Task<OrderToReturnDTo> GetOrderByIdAsync(Guid id);
    }
}
