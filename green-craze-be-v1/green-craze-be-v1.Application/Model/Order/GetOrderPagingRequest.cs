using green_craze_be_v1.Application.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Order
{
    public class GetOrderPagingRequest : PagingRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public string OrderStatus { get; set; }
    }
}