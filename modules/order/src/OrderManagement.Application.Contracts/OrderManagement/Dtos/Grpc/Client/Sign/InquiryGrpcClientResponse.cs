using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client.Sign
{
    public class InquiryGrpcClientResponse
    {
        public string Message { get; set; }
        public int ResultCode { get; set; }
        public bool Success { get; set; }
        public string State { get; set; }
        public string DocumentLink { get; set; }
        public string SignedDocumentLink { get; set; }
    }
}
