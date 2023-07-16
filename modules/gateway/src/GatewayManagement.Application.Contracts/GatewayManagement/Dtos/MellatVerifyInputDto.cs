namespace GatewayManagement.Application.Contracts.Dtos
{
    public class MellatVerifyInputDto
    {
        public long TerminalId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public long OrderId { get; set; }
        public long SaleOrderId { get; set; }
        public long SaleReferenceId { get; set; }
        public int Switch { get; set; }
    }
}
