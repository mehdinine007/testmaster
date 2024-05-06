using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class WorkFlowRecipients
    {
        public int recipientOrder { get; set; }
        public string recipientUsername { get; set; }
        public string recipientRole { get; set; }
        public Reminder reminder { get; set; }
        public Certificate certificate { get; set; }
        public RecipientConfig recipientConfig { get; set; }
        public DocumentParameter documentParameter { get; set; }
    }
}
