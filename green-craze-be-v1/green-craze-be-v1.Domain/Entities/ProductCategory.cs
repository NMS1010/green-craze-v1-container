using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class ProductCategory : BaseAuditableEntity<long>
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public long? ParentId { get; set; }
        public string Slug { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<Product> Products { get; set; }
    }
}