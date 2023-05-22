using PaymentManagement.Domain.Models;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace PaymentManagement.Application.IranKish
{
    public class BackFromPspResult
    {
        public string token { get; set; }
        public string acceptorId { get; set; }
        public string responseCode { get; set; }
        public string paymentId { get; set; }
        public string requestId { get; set; }
        public string maskedPan { get; set; }
        public string sha256OfPan { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public string systemTraceAuditNumber { get; set; }
        public string amount { get; set; }
        public string isMigratedMerchant { get; set; }
        public string merchantID { get; set; }
        public string ttl { get; set; }
        public string sha1OfPan { get; set; }
        public string transactionType { get; set; }
        public string OriginUrl { get;  set; }
    }
}
