namespace PaymentManagement.Application.Pasargad
{
    public class BackFromPspResult
    {
        public string Status { get; set; }
        public string InvoiceId { get; set; }
        public string ReferenceNumber { get; set; }
        public string TrackId { get; set; }
        public string OriginUrl { get; set; }
    }
}
