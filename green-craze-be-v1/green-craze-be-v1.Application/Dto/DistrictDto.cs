using green_craze_be_v1.Domain.Common;
using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class DistrictDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ProvinceDto Province { get; set; }
    }
}