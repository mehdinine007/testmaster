namespace GatewayManagement.Application.Contracts.Dtos
{
    public class ParsianHandShakeInputDto
    {
        public string LoginAccount { get; set; }
        public string CallBackUrl { get; set; }
        public long Amount { get; set; }
        public int OrderId { get; set; }
        public string AdditionalData { get; set; }
        public string Originator { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
        public int ThirdPartyCode { get; set; }
    }
}
