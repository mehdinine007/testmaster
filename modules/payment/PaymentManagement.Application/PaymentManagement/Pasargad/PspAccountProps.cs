namespace PaymentManagement.Application.Pasargad
{
    public class PspAccountProps
    {
        public int TerminalNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
        public int ThirdPartyCode { get; set; }
    }
}
