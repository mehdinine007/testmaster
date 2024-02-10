using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Dtos
{
    public class IPgCallBackLog 
    {
        public string Description { get; set; }
        public Dictionary<string,object> Data { get; set; }
    }

    public class IPgCallBackLogData : IPgCallBackRequest
    {
        public int OrderId { get; set; }
    }



}
