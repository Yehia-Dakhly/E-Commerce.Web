using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public sealed class NotFoundOrderException(Guid id) : NotFoundException($"Order With Id:{id} Not Found!")
    {
    }
}
