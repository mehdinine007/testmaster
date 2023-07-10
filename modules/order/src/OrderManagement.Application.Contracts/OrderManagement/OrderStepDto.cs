using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class OrderStepDto
    {
        public DateTime StartTime { get; set; }
        public OrderStepEnum Step { get; set; }
    }
}
