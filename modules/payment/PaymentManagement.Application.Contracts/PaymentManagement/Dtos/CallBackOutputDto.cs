namespace PaymentManagement.Application.Contracts.Dtos
{
    public class CallBackOutputDto
    {
        public string CallBackUrl { get; set; }
        public string CustomerAuthorizationToken { get; set; }
    }
}