namespace GatewayManagement.Application.Contracts.Dtos
{
    public class PasargadHandShakeInputDto
    {
        public string Token { get; set; }
        public long Amount { get; set; }
        public string CallbackApi { get; set; }
        public string Description { get; set; }
        public string Invoice { get; set; }
        public string InvoiceDate { get; set; }
        public string MobileNumber { get; set; }
        public string PayerMail { get; set; }
        public string PayerName { get; set; }
        public int ServiceCode { get; set; }
        public string ServiceType { get; set; }
        public int TerminalNumber { get; set; }
        public string NationalCode { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
        public string Pans { get; set; }
    }
}
