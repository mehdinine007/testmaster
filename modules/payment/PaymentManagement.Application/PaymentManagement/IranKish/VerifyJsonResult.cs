namespace PaymentManagement.Application.IranKish
{
    public class VerifyJsonResult
    {
        public string responseCode { get; set; }
        public string description { get; set; }
        public bool status { get; set; }
        public VerifyResult result { get; set; }
    }

    public class VerifyResult
    {
        public string responseCode { get; set; }
        public string systemTraceAuditNumber { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public DateTime transactionDateTime { get; set; }
        public int transactionDate { get; set; }
        public int transactionTime { get; set; }
        public string processCode { get; set; }
        public string requestId { get; set; }
        public object additional { get; set; }
        public object billType { get; set; }
        public object billId { get; set; }
        public object paymentId { get; set; }
        public string amount { get; set; }
        public bool duplicateVerify { get; set; }
        public object revertUri { get; set; }
        public string acceptorId { get; set; }
        public string terminalId { get; set; }
        public string tokenIdentity { get; set; }
    }
}
