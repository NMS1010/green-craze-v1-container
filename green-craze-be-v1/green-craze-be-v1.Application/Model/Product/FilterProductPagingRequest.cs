using green_craze_be_v1.Application.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Product
{
    public class FilterProductPagingRequest : PagingRequest
    {
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = decimal.MaxValue;
        public List<long> BrandIds { get; set; } = new List<long>();
        public List<long> CategoryIds { get; set; } = new List<long>();
        public string CategorySlug { get; set; }
        public int? Rating { get; set; }
    }
}