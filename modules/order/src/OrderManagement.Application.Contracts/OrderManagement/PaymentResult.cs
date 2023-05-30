namespace OrderManagement.Application.Contracts
{
    public class PaymentResult : IPaymentResult
    {
        public int Status { get; set; }
        public int PaymentId { get; set; }
        public string Message { get; set; }
        public int OrderId { get; set; }
    }
}