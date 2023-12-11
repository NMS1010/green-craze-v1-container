using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class Address : BaseAuditableEntity<long>
    {
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public bool IsDefault { get; set; }
        public bool Status { get; set; } = true;
        public Province Province { get; set; }
        public District District { get; set; }
        public Ward Ward { get; set; }
        public AppUser User { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}