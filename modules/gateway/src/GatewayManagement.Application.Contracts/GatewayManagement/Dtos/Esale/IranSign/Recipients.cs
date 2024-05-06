using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class Recipients
    {
        public string recipientTicket { get; set; }
        public string recipientUserId { get; set; }
        public string recipientName { get; set; }
        public string recipientRole { get; set; }
        public string recipientState { get; set; }
        public int recipientOrder { get; set; }
        public string receivedDate { get; set; }
        public DateTime? runningDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? expirationTimeAfterReceipt { get; set; }
        public string certificateKeyId { get; set; }
        public RecipientConfig recipientConfig { get; set; }
        public RecipientReminder recipientReminder { get; set; }
        public RecipientDocumentParameter recipientDocumentParameter { get; set; }
        public string recipientSignedData { get; set; }
        public bool seen { get; set; }
    }
}
