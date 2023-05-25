namespace OrderManagement.Application.Contracts
{
    public class PspHandShakeRequest
    {
        public int PspAccountId { get; set; }
        public long Amount { get; set; }
        public string CallBackUrl { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public int FilterParam { get; set; }
    }
}