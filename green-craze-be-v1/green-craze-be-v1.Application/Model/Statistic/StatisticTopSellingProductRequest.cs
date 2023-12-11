using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Statistic
{
    public class StatisticTopSellingProductRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Top { get; set; } = 5;
    }
}