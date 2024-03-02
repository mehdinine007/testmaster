namespace GatewayManagement.Application.Contracts.Dtos
{
    public class IranKishHandShakeInputDto
    {
        public string TerminalId { get; set; }
        public string AcceptorId { get; set; }
        public string PassPhrase { get; set; }
        public string CallBackUrl { get; set; }
        public long Amount { get; set; }
        public string RequestId { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public string RsaPublicKey { get; set; }
    }
}
