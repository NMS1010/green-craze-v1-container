using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Review
{
    public class CreateReviewRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public long ProductId { get; set; }
        public long OrderItemId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public IFormFile Image { get; set; }
    }
}
