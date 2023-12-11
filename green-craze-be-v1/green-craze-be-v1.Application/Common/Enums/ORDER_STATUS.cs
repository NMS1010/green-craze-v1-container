using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Common.Enums
{
    public class ORDER_STATUS
    {
        public static string NOT_PROCESSED = "NOT_PROCESSED";
        public static string PROCESSING = "PROCESSING";
        public static string SHIPPED = "SHIPPED";
        public static string DELIVERED = "DELIVERED";
        public static string CANCELLED = "CANCELLED";

        public static List<string> Status = new List<string>()
        {
            NOT_PROCESSED,
            PROCESSING,
            SHIPPED,
            DELIVERED,
            CANCELLED
        };

        public static Dictionary<string, string> OrderStatusSubTitle = new()
        {
            {NOT_PROCESSED, "Chưa xử lý"},
            {PROCESSING, "Đang được xử lý"},
            {SHIPPED, "Đang được vận chuyển"},
            {DELIVERED, "Đã giao hàng"},
            {CANCELLED, "Bị huỷ"},
        };
    }
}