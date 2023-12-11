using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.ProductImage
{
    public class CreateProductImageRequest
    {
        public IFormFile Image {  get; set; }
        public long ProductId { get; set; }
    }
}
