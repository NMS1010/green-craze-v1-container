using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class Product : BaseAuditableEntity<long>
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public long? Quantity { get; set; }
        public long? ActualInventory {  get; set; }
        public long? Sold { get; set; }
        public string Status { get; set; }
        public string Slug { get; set; }
        public double? Rating { get; set; }
        public decimal Cost { get; set; }
        public Unit Unit { get; set; }
        public ProductCategory Category { get; set; }
        public Brand Brand { get; set; }
        public Sale Sale { get; set; }
        public ICollection<UserFollowProduct> UserFollowProducts { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<Variant> Variants { get; set; } = new List<Variant>();
        public ICollection<Docket> Dockets { get; set; } = new List<Docket>();
    }
}