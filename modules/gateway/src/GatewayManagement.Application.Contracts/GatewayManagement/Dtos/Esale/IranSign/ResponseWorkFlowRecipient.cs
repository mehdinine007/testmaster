using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class ResponseWorkFlowRecipient
    {
        public string RecipientTicket { get; set; }
        public string RecipientName { get; set; }
        public string RecipientRole { get; set; }
        public int RecipientOrder { get; set; }
    }
}
