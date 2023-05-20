using System.Runtime.Serialization;

namespace PaymentManagement.Application.IranKish
{
    public class RequestVerify
    {
        [DataMember]
        public string terminalId { get; set; }

        [DataMember]
        public string retrievalReferenceNumber { get; set; }

        [DataMember]
        public string systemTraceAuditNumber { get; set; }

        [DataMember]
        public string tokenIdentity { get; set; }
    }
}
