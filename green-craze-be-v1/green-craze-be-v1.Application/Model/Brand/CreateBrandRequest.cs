using Microsoft.AspNetCore.Http;

namespace green_craze_be_v1.Application.Model.Brand
{
    public class CreateBrandRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image {  get; set; }
        public string Code { get; set; }
    }
}
