namespace OrderManagement.Application.Contracts
{
    public class HandShakeResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get;set; }
        public int PaymentId { get; set; }
        public string Token { get; set; }
        public string Url{get;set;}
        public string PspJsonResult{get;set;}
    }
}