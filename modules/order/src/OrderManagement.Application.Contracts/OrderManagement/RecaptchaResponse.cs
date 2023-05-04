namespace OrderManagement.Application.Contracts
{
    public class RecaptchaResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string ErrorCode { get; set; }
    }
}