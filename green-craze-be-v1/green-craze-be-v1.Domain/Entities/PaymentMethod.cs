using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class PaymentMethod : BaseAuditableEntity<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; } = true;
        public string Image { get; set; }
    }
}