using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Auth
{
    public class ResendOTPRequest
    {
        public string Email { get; set; }

        [JsonIgnore]
        public string Type { get; set; }
    }
}