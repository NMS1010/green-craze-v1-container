using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Common.Enums
{
    public class PRODUCT_STATUS
    {
        public const string ACTIVE = "ACTIVE";
        public const string INACTIVE = "INACTIVE";
        public const string SOLD_OUT = "SOLD_OUT";

        public static List<string> Status = new()
        {
            ACTIVE,
            INACTIVE,
            SOLD_OUT
        };
    }
}