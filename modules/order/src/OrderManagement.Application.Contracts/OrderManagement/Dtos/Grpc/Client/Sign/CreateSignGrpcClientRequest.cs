using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client.Sign
{
    public class CreateSignGrpcClientRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
        public string DocumentData { get; set; }
        public string RecipientUsername { get; set; }
        public string DocumentParameter { get; set; }
    }
}
