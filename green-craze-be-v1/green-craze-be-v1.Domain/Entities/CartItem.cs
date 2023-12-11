using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class CartItem : BaseAuditableEntity<long>
    {
        public Cart Cart { get; set; }
        public Variant Variant { get; set; }
        public int Quantity { get; set; }
    }
}