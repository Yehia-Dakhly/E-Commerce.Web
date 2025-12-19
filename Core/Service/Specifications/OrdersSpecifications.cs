using DomainLayer.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class OrdersSpecifications : BaseSpecifications<Order, Guid>
    {
        public OrdersSpecifications(string Email) : base(O => O.BuyerEmail == Email)
        {
            AddInclude(P => P.DeliveryMethod);
            AddInclude(P => P.Items);
            AddOrderByDescending(P => P.OrderDate);

        }
        public OrdersSpecifications(Guid guid) : base(O => O.Id == guid)
        {
            AddInclude(P => P.DeliveryMethod);
            AddInclude(P => P.Items);
        }
    }
}
