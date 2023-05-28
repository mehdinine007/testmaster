using System.ComponentModel;

namespace PaymentManagement.Application.Contracts.Enums
{
    public enum StatusCodeEnum : int
    {
        [Description("موفق")]
        Success = 0,
        [Description("ناموفق")]
        Failed = 1,
        [Description("نامشخص")]
        Unknown = 2

        //todo:نیاز به ارسال این وضعیت مجددا چک شود
        //[Description("پرداخت موفق")]
        //PaymentSuccess = 2,
    }
}
