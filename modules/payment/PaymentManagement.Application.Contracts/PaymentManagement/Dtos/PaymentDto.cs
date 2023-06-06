namespace PaymentManagement.Application.Contracts.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int PspAccountId { get; set; }
        public int PaymentStatusId { get; set; }
        public string Token { get; set; }
        public string TransactionCode { get; set; }
        public string TraceNo { get; set; }
        public int RetryCount { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public decimal Amount { get; set; }
        public string AdditionalData { get; set; }
        public int? FilterParam1 { get; set; }
        public int? FilterParam2 { get; set; }
        public int? FilterParam3 { get; set; }
        public int? FilterParam4 { get; set; }
    }
}
