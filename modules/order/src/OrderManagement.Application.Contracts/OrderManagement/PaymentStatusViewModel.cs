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
        public int? F1 { get; set; }
        public int? F2 { get; set; }
        public int? F3 { get; set; }
        public int? F4 { get; set; }

    }
}
