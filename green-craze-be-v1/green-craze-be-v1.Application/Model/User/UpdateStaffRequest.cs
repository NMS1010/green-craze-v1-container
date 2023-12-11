using green_craze_be_v1.Application.Model.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.User
{
    public class UpdateStaffRequest : UpdateUserRequest
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string Type { get; set; }
        public string Password { get; set; }
        public UpdateAddressRequest Address { get; set; }
    }
}