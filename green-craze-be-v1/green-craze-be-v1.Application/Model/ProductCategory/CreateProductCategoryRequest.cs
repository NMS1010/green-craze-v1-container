using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.ProductCategory
{
    public class CreateProductCategoryRequest 
    {
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public IFormFile Image { get; set; }
        public string Slug { get; set; }
    }
}
