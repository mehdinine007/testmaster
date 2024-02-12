using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class IPgCallBackLogData : IPgCallBackRequest
    {
        public int OrderId { get; set; }
    }
}
