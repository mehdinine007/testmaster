namespace PaymentManagement.Application.Parsian
{
    public class PspAccountProps
    {
        public string TerminalNo { get; set; }
        public string LoginAccount { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
        public int ThirdPartyCode { get; set; }
        public string ReportServiceUserName { get; set; }
        public string ReportServicePassword { get; set; }
    }
}
