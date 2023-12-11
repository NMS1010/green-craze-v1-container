using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class AppUserToken : BaseAuditableEntity<long>
    {
        public string Token { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string Type { get; set; }
        public AppUser User { get; set; }
    }
}