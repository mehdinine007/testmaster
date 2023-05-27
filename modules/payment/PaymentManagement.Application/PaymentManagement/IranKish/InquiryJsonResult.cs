namespace PaymentManagement.Application.IranKish
{
    public class InquiryJsonResult
    {
        public string responseCode { get; set; }
        public string description { get; set; }
        public bool status { get; set; }
        public InquiryResult result { get; set; }
    }

    public class InquiryResult
    {
        public string tokenIdentity { get; set; }
        public string terminalId { get; set; }
        public string acceptorId { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public string systemTraceAuditNumber { get; set; }
        public int amount { get; set; }
        public int transactionDate { get; set; }
        public int transactionTime { get; set; }
        public string requestId { get; set; }
        public object paymentId { get; set; }
        public bool isMultiplex { get; set; }
        public bool isVerified { get; set; }
        public bool isReversed { get; set; }
        public string maskedPan { get; set; }
        public string sha256OfPan { get; set; }
        public string responseCode { get; set; }
        public string transactionType { get; set; }
    }
}
