using PaymentManagement.Domain.Models;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace PaymentManagement.Application.IranKish
{
    public class BackFromPspResult
    {
        public int RequestId { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public string responseCode { get; set; }
        public string UrlReferrer { get;  set; }
        public string UrlReferrerAuthority { get;  set; }
    }
}
