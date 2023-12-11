using green_craze_be_v1.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Review
{
    public class ReviewDto : BaseAuditableDto<long>
    {
        public string UserId { get; set; }
        public long ProductId { get; set; }
        public long OrderItemId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public string Image { get; set; }
        public string Reply { get; set; }
        public bool Status { get; set; }
        public string VariantName { get; set; }
        public ProductDto Product { get; set; }
        public UserDto User { get; set; }
    }
}
