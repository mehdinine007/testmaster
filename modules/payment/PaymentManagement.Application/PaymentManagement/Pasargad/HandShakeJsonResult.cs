namespace PaymentManagement.Application.Pasargad
{
    public class HandShakeJsonResult
    {
        public string ResultMsg { get; set; }
        public int ResultCode { get; set; }
        public Data Data { get; set; }
    }
    public class Data
    {
        public string UrlId { get; set; }
        public string Url { get; set; }
    }
}
