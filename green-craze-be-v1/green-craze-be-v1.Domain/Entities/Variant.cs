using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class Variant : BaseAuditableEntity<long>
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal? PromotionalItemPrice { get; set; }
        public decimal? TotalPromotionalPrice { get; set; }
        public string Status { get; set; }
        public long Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public Product Product { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}