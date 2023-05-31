namespace OrderManagement.Application.Contracts
{
    public class PspHandShakeRequest
    {
        public int PspAccountId { get; set; }
        public long Amount { get; set; }
        public string CallBackUrl { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public int FilterParam1 { get; set; } 
        public int FilterParam2 { get; set; }
        public int FilterParam3 { get; set; }
        public int FilterParam4 { get; set; }
        public string CustomerAuthorizationToken { get; set; }
    }
}