using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client.Sign
{
    public class InquirySignGrpcClientResponse
    {
        public string message { get; set; }
        public int resultCode { get; set; }
        public string responseBody { get; set; }
    }
}
