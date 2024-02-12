namespace PaymentManagement.Application.Parsian
{
    public class VerifyJsonResult
    {
        public short Status { get; set; }
        public string CardNumberMasked { get; set; }
        public long RRN { get; set; }
        public long Token { get; set; }
    }
}
