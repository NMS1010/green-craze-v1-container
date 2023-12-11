using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class DocketDto : BaseAuditableDto<long>
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public long? OrderId { get; set; }
        public long Quantity { get; set; }
        public string Note { get; set; }
    }
}
