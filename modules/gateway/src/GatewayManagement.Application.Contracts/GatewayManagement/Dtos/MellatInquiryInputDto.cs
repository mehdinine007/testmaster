namespace GatewayManagement.Application.Contracts.Dtos
{
    public class MellatInquiryInputDto
    {
        public long TerminalId { get; set; }
        public string ReportServiceUserName { get; set; }
        public string ReportServicePassword { get; set; }
        public long OrderId { get; set; }
        public int Switch { get; set; }
    }
}
