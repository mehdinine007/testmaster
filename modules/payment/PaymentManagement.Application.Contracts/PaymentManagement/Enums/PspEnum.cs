using System.ComponentModel;

namespace PaymentManagement.Application.Contracts.Enums
{
    public enum PspEnum : int
    {
        [Description("به پرداخت ملت")]        
        BehPardakht = 1,
        [Description("ایران کیش")]
        IranKish = 2
    }
}
