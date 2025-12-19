using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.OrderModuleDTos
{
    public class OrderItemDTo
    {

        public string ProductName { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
