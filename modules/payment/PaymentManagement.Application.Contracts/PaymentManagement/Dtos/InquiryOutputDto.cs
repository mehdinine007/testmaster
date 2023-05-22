namespace PaymentManagement.Application.Contracts
{
    public class InquiryOutputDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int PaymentId { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusDescription { get; set; }
        public string PspJsonResult { get; set; }
    }
}