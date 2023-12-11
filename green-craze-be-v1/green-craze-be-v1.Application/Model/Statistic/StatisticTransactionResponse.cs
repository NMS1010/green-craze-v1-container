using green_craze_be_v1.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Statistic
{
    public class StatisticTransactionResponse
    {
        public TransactionDto Transaction { get; set; }
        public UserDto User { get; set; }

        public StatisticTransactionResponse(TransactionDto transaction, UserDto user)
        {
            Transaction = transaction;
            User = user;
        }
    }
}
