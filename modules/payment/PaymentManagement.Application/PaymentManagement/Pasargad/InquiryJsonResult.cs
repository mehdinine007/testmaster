namespace PaymentManagement.Application.Pasargad.Inquiry
{
    public class InquiryJsonResult
    {
        public string ResultMsg { get; set; }
        public int ResultCode { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public int Status { get; set; }
        public string TrackId { get; set; }
        public string TransactionId { get; set; }
        public long Amount { get; set; }
        public string CardNumber { get; set; }
        public string Invoice { get; set; }
        public string Url { get; set; }
        public string ReferenceNumber { get; set; }
        public string RequestDate { get; set; }
    }
}