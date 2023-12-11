using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Statistic
{
    public class StatisticTotalResponse
    {
        public decimal Revenue { get; set; }
        public decimal Expense {  get; set; }
        public int Users {  get; set; }
        public int Orders { get; set; }

        public StatisticTotalResponse(decimal revenue, decimal expense, int users, int orders)
        {
            Revenue = revenue;
            Expense = expense;
            Users = users;
            Orders = orders;
        }
    }
}
