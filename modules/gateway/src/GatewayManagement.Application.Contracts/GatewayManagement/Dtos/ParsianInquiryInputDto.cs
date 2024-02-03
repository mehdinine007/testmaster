namespace GatewayManagement.Application.Contracts.Dtos
{
    public class ParsianInquiryInputDto
    {
        public int OrderId { get; set; }
        public long Token { get; set; }
        public string ReportServiceUserName { get; set; }
        public string ReportServicePassword { get; set; }
    }
}
