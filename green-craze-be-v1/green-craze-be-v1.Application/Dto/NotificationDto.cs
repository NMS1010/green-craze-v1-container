using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class NotificationDto : BaseAuditableDto<long>
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Anchor { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}