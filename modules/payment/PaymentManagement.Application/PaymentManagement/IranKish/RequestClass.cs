using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Application.IranKish
{
    public class RequestClass
    {
        public AuthenticationEnvelope AuthenticationEnvelope = new AuthenticationEnvelope();
        public Request Request = new Request();
    }
    public class AuthenticationEnvelope
    {
        public string Data { get; set; }
        public string Iv { get; set; }
    }
    public class Request
    {
        public string AcceptorId { get; set; }
        public long amount { get; set; }
        public BillInfo BillInfo { get; set; }
        public string CmsPreservationId { get; set; }
        public List<MultiplexParameter> multiplexParameters { get; set; }
        public string PaymentId { get; set; }
        public string RequestId { get; set; }
        public long RequestTimestamp { get; set; }
        public string RevertUri { get; set; }
        public string terminalId { get; set; }
        public string transactionType { get; set; }
        public string nationalId { get; set; }
        public List<KeyValuePair<string, string>> additionalParameters { get; set; }
    }
}
