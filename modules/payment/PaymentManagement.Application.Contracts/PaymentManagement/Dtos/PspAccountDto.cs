namespace PaymentManagement.Application.Contracts.Dtos
{
    public class PspAccountDto
    {
        public int Id { get; set; }
        public int PspId { get; set; }
        public string Psp { get; set; }
        public string AccountName { get; set; }
    }
}