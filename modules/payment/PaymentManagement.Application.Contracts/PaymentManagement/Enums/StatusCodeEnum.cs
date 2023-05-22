namespace PaymentManagement.Application.Contracts.Enums
{
    public enum StatusCodeEnum : int
    {
        /// <summary>
        /// موفق
        /// </summary>
        Success = 0,
        /// <summary>
        /// ناموفق
        /// </summary>
        Failed = 1,
        /// <summary>
        /// پرداخت موفق
        /// </summary>
        PaymentSuccess = 2,
        /// <summary>
        /// پرداخت ناموفق
        /// </summary>
        PaymentFailed = 3,
    }
}
