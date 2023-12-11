using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Order
{
    public class CompletePaypalOrderRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }

        [JsonIgnore]
        public long OrderId { get; set; }

        public string PaypalOrderId { get; set; } = null;
        public string PaypalOrderStatus { get; set; } = null;
    }
}