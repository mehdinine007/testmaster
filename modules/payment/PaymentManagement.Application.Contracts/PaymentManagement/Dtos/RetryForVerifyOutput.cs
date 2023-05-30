namespace PaymentManagement.Application.Contracts.Dtos
{
    public class RetryForVerifyOutput
    {
        public int PaymentId { get; set; }
        public int StatusId { get; set; }
    }

    public class RetryForVerifyDetail : RetryForVerifyOutput
    {
        public int? FilterParam1 { get; set; }
        public int? FilterParam2 { get; set; }
        public int? FilterParam3 { get; set; }
        public int? FilterParam4 { get; set; }
    }

}
