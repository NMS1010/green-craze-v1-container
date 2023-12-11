using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class UserDto : BaseAuditableDto<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
        public List<AddressDto> Addresses { get; set; }
    }
}