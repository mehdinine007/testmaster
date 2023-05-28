namespace OrderManagement.Application.Contracts
{
    public class HandShakeResultDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int PaymentId { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
    }
}