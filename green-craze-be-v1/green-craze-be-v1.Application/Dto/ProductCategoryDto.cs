using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class ProductCategoryDto : BaseAuditableDto<long>
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string Image {  get; set; }
        public string Slug { get; set; }
        public bool Status { get; set; }
    }
}
