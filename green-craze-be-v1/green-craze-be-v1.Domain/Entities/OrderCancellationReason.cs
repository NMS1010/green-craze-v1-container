using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class OrderCancellationReason : BaseAuditableEntity<long>
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<Order> Orders { get; set; }
    }
}