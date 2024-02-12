namespace PaymentManagement.Application.Parsian
{
    public class InquiryJsonResult
    {
        public InquiryResult Data { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class InquiryResult
    {
        public string LoginAccount { get; set; }
        public string WebSiteAddress { get; set; }
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public long Token { get; set; }
        public string CardNumber { get; set; }
        public int PGWStatusId { get; set; }
        public string RequestDateTime { get; set; }
        public long RRN { get; set; }
        public int PTraceNo { get; set; }
        public string ClientIPAddress { get; set; }
    }
}
