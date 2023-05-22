namespace PaymentManagement.Application.Contracts
{
    public class PspAccountDto
    {
        public int Id { get; set; }
        public string Psp { get; set; }
        public string AccountName { get; set; }
        public string Logo { get; set; }
    }
}