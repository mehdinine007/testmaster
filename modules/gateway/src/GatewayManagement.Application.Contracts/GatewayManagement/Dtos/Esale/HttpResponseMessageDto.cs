using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos
{
    public class HttpResponseMessageDto
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string ErrorCode { get; set; }
    }
}
