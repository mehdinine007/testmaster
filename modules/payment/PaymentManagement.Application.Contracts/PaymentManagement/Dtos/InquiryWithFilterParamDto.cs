namespace PaymentManagement.Application.Contracts
{
    public class InquiryWithFilterParamDto
    {
        public int Status { get; set; }
        public string Message { get; set; }        
        public int Count { get; set; }
    }
}