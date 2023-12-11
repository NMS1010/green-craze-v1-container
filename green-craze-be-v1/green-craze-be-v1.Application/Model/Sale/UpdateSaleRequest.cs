using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Sale
{
    public class UpdateSaleRequest
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public double PromotionalPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public bool All { get; set; }
        public List<long> CategoryIds { get; set; }
    }
}
