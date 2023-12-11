using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.PaymentMethod
{
    public class UpdatePaymentMethodRequest
    {
        [JsonIgnore]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
        public bool Status { get; set; }
        public IFormFile Image { get; set; }
    }
}