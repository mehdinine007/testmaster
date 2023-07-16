namespace GatewayManagement.Application.Contracts.Dtos
{
    public class IranKishInquiryInputDto
    {
        public string TerminalId { get; set; }
        public string PassPhrase { get; set; }
        public string TokenIdentity { get; set; }
        public int FindOption { get; set; }
    }
}
