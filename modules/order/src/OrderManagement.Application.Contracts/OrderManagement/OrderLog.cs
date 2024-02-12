using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class OrderLog
    {
        public string Description { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }

    



}
