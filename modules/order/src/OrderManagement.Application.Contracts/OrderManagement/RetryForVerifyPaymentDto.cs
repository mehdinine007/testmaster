using System.Runtime.Serialization;

namespace OrderManagement.Application.Contracts
{
    public class RetryForVerifyPaymentDto
    {
        public int PaymentId { get; set; }
        public int PaymentStatus { get; set; }
        public int? FilterParam1 { get; set; }
        public int? FilterParam2 { get; set; }
        public int? FilterParam3 { get; set; }
        public int? FilterParam4 { get; set; }
    }
}
