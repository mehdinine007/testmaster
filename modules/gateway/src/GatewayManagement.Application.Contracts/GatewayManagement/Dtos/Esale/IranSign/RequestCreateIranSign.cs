using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class RequestCreateIranSign
    {
        public WorkFlowInfo workflowInfo { get; set; }
        public List<WorkFlowRecipients> workflowRecipients { get; set; }
    }
}
