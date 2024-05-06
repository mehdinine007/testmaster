using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class ResponseBodies
    {
        public string workflowTicket { get; set; }
        public DateTime updateDate { get; set; }
        public DateTime createDate { get; set; }
        public string title { get; set; }
        public string workflowType { get; set; }
        public List<ResponseWorkFlowRecipients> workflowRecipients { get; set; }

    }
}
