namespace GatewayManagement.Application.Contracts.Dtos
{
    public class ParsianInquiryInputDto
    {
        public int OrderId { get; set; }
        public string LoginAccount { get; set; }
        public string ReportServiceUserName { get; set; }
        public string ReportServicePassword { get; set; }
    }
}
