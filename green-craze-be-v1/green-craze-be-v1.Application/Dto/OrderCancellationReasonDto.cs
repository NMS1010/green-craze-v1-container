using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class OrderCancellationReasonDto : BaseAuditableDto<long>
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
    }
}