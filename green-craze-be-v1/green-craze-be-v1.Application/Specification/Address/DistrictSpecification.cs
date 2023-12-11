using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Address
{
    public class DistrictSpecification : BaseSpecification<District>
    {
        public DistrictSpecification(long id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Province);
            AddInclude(x => x.Wards);
        }
    }
}