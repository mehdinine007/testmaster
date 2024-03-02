namespace PaymentManagement.Application.Pasargad.Verify
{
    public class VerifyJsonResult
    {
        public string ResultMsg { get; set; }
        public int ResultCode { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string Invoice { get; set; }
        public string ReferenceNumber { get; set; }
        public string TrackId { get; set; }
        public string MaskedCardNumber { get; set; }
        public string HashedCardNumber { get; set; }
        public string RequestDate { get; set; }
        public int Amount { get; set; }
    }
}