namespace PaymentManagement.Application.Contracts
{
    public class RedirectToPspOutput
    {
        public int PaymentId { get; set; }
        public int PspId { get; set; }
        public string Token { get; set; }
    }
}