namespace PaymentManagement.Application.Parsian
{
    public class InquiryJsonResult
    {
        public InquiryJsonResult()
        {
            Data = new List<InquiryResult>();
        }
        public List<InquiryResult> Data { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class InquiryResult
    {
        public long Amount { get; set; }
        public string MerchantName { get; set; }
        public string WebSiteAddress { get; set; }
        public long RRN { get; set; }
        public long OrderId { get; set; }
        public long Token { get; set; }
        public int PTraceNo { get; set; }
        public int STraceNo { get; set; }
        public int TermNo { get; set; }
        public string RequestDateTime { get; set; }
        public string ClientIPAddress { get; set; }
        public string TruncatedCardNumber { get; set; }
        public short PGWStatusId { get; set; }
        public long? MobileNumber { get; set; }
        public string AdditionalData { get; set; }
        public long? RequesterMobileNo { get; set; }
        public string TransTypeTitle { get; set; }
    }
}
