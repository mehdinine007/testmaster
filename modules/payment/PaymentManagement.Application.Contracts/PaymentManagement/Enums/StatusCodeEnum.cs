using System.ComponentModel;

namespace PaymentManagement.Application.Contracts.Enums
{
    public enum StatusCodeEnum : int
    {
        [Description("موفق")]
        Success = 0,
        [Description("ناموفق")]
        Failed = 1,
        [Description("پرداخت موفق")]
        PaymentSuccess = 2,
        [Description("پرداخت ناموفق")]
        PaymentFailed = 3,
    }
}
