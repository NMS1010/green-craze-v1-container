using green_craze_be_v1.Application.Model.Address;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.User
{
    public class CreateStaffRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public CreateAddressRequest Address { get; set; }
    }
}