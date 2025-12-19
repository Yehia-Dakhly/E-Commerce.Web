using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public sealed class DeliveryMethodNotFound(int Id) : NotFoundException($"Delivery Method With Id:{Id} is Not Found!")
    {

    }
}
