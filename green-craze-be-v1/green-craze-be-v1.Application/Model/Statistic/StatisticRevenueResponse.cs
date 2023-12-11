using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Statistic
{
    public class StatisticRevenueResponse
    {
        public string Date { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expense { get; set; }

        public StatisticRevenueResponse(string date, decimal revenue, decimal expense)
        {
            Date = date;
            Revenue = revenue;
            Expense = expense;
        }
    }
}
