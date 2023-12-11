using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Statistic
{
    public class StatisticOrderStatusResponse
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public StatisticOrderStatusResponse(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
