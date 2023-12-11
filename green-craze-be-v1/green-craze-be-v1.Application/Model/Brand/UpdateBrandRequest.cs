using Microsoft.AspNetCore.Http;

namespace green_craze_be_v1.Application.Model.Brand
{
    public class UpdateBrandRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
    }
}
