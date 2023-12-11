using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Order
{
    public class CreateOrderItemRequest
    {
        public int Quantity { get; set; }
        public long VariantId { get; set; }
    }
}