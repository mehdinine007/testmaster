using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale
{
    public class CreateSignOutputDto
    {
        public string Message { get; set; }
        public int ResultCode { get; set; }
        public bool Success { get; set; }
        public string ResponseBody { get; set; }
        public string WorkflowRecipients { get; set; }
    }
}
