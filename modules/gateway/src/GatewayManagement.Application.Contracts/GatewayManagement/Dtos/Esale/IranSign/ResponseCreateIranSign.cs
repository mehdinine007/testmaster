using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class ResponseCreateIranSign
    {
        public string message { get; set; }
        public int resultCode { get; set; }
        public bool Success { get; set; }
        public List<ResponseBodies> responseBody { get; set; }
        public List<ResponseWorkFlowRecipients> workflowRecipients { get; set; }
    }

    public class CreateIranSignOutput
    {
        public string Message { get; set; }
        public int ResultCode { get; set; }
        public bool Success { get; set; }
        public string ResponseBody { get; set; }
        public string WorkflowRecipients { get; set; }
    }

    public class ErrorResponseIranSign
    {
        public string message { get; set; }
        public string code { get; set; }
       
    }
}
