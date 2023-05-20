namespace PaymentManagement.Application.Contracts.Enums
{
    public enum PaymentStatusEnum : int
    {
        //todo:لیست اوکی هست؟
        /// <summary>
        /// در حال پرداخت
        /// </summary>
        Inprogress = 1,
        /// <summary>
        /// پرداخت موفق
        /// </summary>
        Success,
        /// <summary>
        /// پرداخت ناموفق
        /// </summary>
        Failed,
    }
}
