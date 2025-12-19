using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class UserNotFoundException(string Msg) :NotFoundException($"Not Found User With Email:{Msg}!")
    {
    }
}
