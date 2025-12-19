using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.OrderModuleDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Create Order
        
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTo>> CreateOrder(OrderDTo orderDTo)
        {
            return Ok(await _serviceManager.OrderService.CreateOrderAsync(orderDTo, GetEmailFromToken()));
        }
        // Get All Delivery Methods
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTo>>> GetDeliveryMethods()
        {
            return Ok(await _serviceManager.OrderService.GetAllDeliveryMethodsAsync());
        }
        // Get All Orders By Email
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTo>>> GetAllOrders()
        {
            return Ok(await _serviceManager.OrderService.GetAllOrdersByEmailAsync(GetEmailFromToken()));
        }
        // Get Order By Id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTo>> GetOrderById(Guid id)
        {
            return Ok(await _serviceManager.OrderService.GetOrderByIdAsync(id));
        }
    }
}
