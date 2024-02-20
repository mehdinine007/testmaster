using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client
{
    public class PaypaidPriceDto
    {
        public DateTime TranDate { get; set; }
        public long PayedPrice { get; set; }
       
    }
}
