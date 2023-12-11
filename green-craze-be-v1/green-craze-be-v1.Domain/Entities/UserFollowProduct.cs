using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class UserFollowProduct : BaseAuditableEntity<long>
    {
        public AppUser User { get; set; }
        public Product Product { get; set; }
    }
}