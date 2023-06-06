namespace OrderManagement.Application.Contracts
{
    public class IPgCallBackRequest : PspInteractionResult
    {
        public string TransactionCode { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int PaymentId { get; set; }
        public string AdditionalData { get; set; }
        public string PspJsonResult { get; set; }
    }
}