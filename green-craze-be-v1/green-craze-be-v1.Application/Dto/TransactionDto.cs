using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class TransactionDto : BaseAuditableDto<long>
    {
        public string PaymentMethod { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal TotalPay { get; set; }
        public string PaypalOrderId { get; set; }
        public string PaypalOrderStatus { get; set; }
        public string OrderCode { get; set; }
    }
}