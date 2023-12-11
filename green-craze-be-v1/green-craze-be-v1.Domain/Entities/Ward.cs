using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class Ward : BaseAuditableEntity<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long DistrictId { get; set; }
        public District District { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}