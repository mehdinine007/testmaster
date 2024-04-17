using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class ResponseWorkFlowRecipients
    {
        public string recipientTicket { get; set; }
        public string recipientName { get; set; }
        public string recipientRole { get; set; }
        public int recipientOrder { get; set; }
    }
}
