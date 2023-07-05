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

        public static string NoCapacityCreateTicket = "در حال حاضر كل ظرفيت اين برنامه فروش، در حال خريد مي باشد، در صورت آزاد شدن ظرفيت و انصراف مشتريان در صف خريد،  مي توانيد مجددا براي سفارش خودرو تلاش فرماييد";
        public static string NoCapacityCreateTicketId = "1002";

        public static string AgancyNoCapacityCreateTicket = "در حال حاضر كل ظرفيت اين برنامه فروش در نمایندگی، در حال خريد مي باشد، در صورت آزاد شدن ظرفيت و انصراف مشتريان در صف خريد،  مي توانيد مجددا براي سفارش خودرو تلاش فرماييد";
        public static string AgancyNoCapacityCreateTicketId = "1003";

        public static string NoCapacitySaleDetail = "اتمام ظرفیت برنامه فروش!";
        public static string NoCapacitySaleDetailId = "1004";

    }
}
