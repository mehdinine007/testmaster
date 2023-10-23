using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos
{
    public class SendBoxServiceDto
    {
        public bool Success { get; set; }
        public string DataResult { get; set; }
        public string Message { get; set; }
        public int MessageCode { get; set; }
    }
}
