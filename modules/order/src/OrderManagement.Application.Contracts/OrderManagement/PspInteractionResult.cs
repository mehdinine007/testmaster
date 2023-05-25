namespace OrderManagement.Application.Contracts
{
    public class PspInteractionResult
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public string PaymentId { get; set; }
        public string PspJsonResult { get; set; }
    }
}