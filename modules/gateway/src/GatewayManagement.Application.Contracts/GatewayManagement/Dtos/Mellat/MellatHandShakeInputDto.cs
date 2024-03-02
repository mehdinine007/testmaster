namespace GatewayManagement.Application.Contracts.Dtos
{
    public class MellatHandShakeInputDto
    {
        public long TerminalId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public string CallBackUrl { get; set; }
        public string MobileNo { get; set; }
        public string EncryptedNationalCode { get; set; }
        public int Switch { get; set; }
    }
}
