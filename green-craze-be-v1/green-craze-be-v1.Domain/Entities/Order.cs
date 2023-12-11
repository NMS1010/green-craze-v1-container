using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class Order : BaseAuditableEntity<long>
    {
        public string OtherCancelReason { get; set; }
        public decimal TotalAmount { get; set; }
        public double Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public bool PaymentStatus { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string DeliveryMethod { get; set; }
        public AppUser User { get; set; }
        public Address Address { get; set; }
        public Transaction Transaction { get; set; }
        public OrderCancellationReason CancelReason { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Docket> Dockets { get; set; }
    }
}