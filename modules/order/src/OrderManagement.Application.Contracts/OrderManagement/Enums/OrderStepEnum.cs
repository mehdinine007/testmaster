using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Application.Contracts
{
    public enum OrderStepEnum
    {
        Start = 1,
        SubmitOrder = 2,
        PreviewOrder = 3,  
        SaveOrder = 4
    }
}
