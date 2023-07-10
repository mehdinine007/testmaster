using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement
{
    public  class CapacityControlConstants
    {
        public const string  SaleDetailPrefix = "{0}_cc";
        public const string  CapacityControlPrefix = "n:CapacityControl:{0}";
        public static string PaymentCountPrefix = "{0}_pcc";
        public static string PaymentRequestPrefix = "{0}_pcc_SaleReq";
        public const string  AgancySaleDetailPrefix = "{0}_{1}_acc";
        public static string AgancyPaymentPrefix = "{0}_{1}_pacc";
    }
}
