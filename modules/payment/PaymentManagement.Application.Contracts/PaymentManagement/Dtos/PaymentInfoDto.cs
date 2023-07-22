namespace PaymentManagement.Application.Contracts.Dtos
{
    public class PaymentInfoDto
    {
        public int PaymentId { get; set; }
        public string TransactionCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionPersianDate { get; set; }
        public int PaymentStatusId { get; set; }
    }
}
