using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class Review : BaseAuditableEntity<long>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public string Image { get; set; }
        public string Reply { get; set; }
        public bool Status { get; set; } = true;
        public Product Product { get; set; }
        public AppUser User { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}