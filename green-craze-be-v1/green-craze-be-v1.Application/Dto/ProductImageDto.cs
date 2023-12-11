using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class ProductImageDto : BaseAuditableDto<long>
    {
        public string Image { get; set; }
        public long ProductId { get; set; }
        public double Size { get; set; }
        public string ContentType { get; set; }
        public bool IsDefault { get; set; }
    }
}
