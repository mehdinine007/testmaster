namespace GatewayManagement.Application.Contracts.Dtos
{
    public class IranKishVerifyInputDto
    {
        public string TerminalId { get; set; }
        public string RetrievalReferenceNumber { get; set; }
        public string SystemTraceAuditNumber { get; set; }
        public string TokenIdentity { get; set; }
    }
}
