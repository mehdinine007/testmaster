using System.ComponentModel;

namespace PaymentManagement.Application.Contracts.Enums
{
    public enum PaymentStatusEnum : int
    {
        [Description("درحال پرداخت")]        
        InProgress = 1,
        [Description("پرداخت موفق")]
        Success,
        [Description("پرداخت ناموفق")]       
        Failed,
    }
}
