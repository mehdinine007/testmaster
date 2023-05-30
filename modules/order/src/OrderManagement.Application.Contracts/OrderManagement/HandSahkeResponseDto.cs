namespace OrderManagement.Application.Contracts
{
    public class HandShakeResponseDto
    {
        public IpgApiResult Result { get; set; }

        public bool Success { get; set; }

        public object Error { get; set; }
    }

    public class IpgApiResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int PaymentId { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
        public string PspJsonResult { get; set; }
    }
}