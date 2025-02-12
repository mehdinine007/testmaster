﻿using System.ComponentModel;

namespace PaymentManagement.Application.Contracts.Enums
{
    public enum PspEnum : int
    {
        [Description("به پرداخت ملت")]        
        Mellat = 1,
        [Description("ایران کیش")]
        IranKish = 2,
        [Description("تجارت الکترونیک پارسیان")]
        Parsian = 3,
        [Description("تجارت الکترونیک پاسارگاد")]
        Pasargad = 4
    }
}
