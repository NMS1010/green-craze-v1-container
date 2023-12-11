using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class PaymentMethodDto : BaseAuditableDto<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
    }
}