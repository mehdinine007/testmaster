namespace PaymentManagement.Application.Parsian
{
    public class BackFromPspResult
    {
        public string OrderId { get; set; }
        public string Token { get; set; }
        public string status { get; set; }
        public string TerminalNo { get; set; }
        public string RRN { get; set; }
        public string Amount { get; set; }
        public string SwAmount { get; set; }
        public string HashCardNumber { get; set; }
        public string STraceNo { get; set; }
        public string DiscoutedProduct { get; set; }
        public string OriginUrl { get; set; }
    }
}
