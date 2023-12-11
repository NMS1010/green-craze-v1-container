using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class RoleDto : BaseAuditableDto<string>
    {
        public string Name { get; set; }
    }
}