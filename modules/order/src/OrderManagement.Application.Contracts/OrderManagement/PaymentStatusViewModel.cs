using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static OrderManagement.Application.Contracts.OrderManagementPermissions;

namespace OrderManagement.Application.Contracts
{
    public class PaymentStatusModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public long Count { get; set; }
    }
}
