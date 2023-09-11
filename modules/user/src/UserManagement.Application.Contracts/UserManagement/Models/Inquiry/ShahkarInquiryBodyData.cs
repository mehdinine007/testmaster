namespace UserManagement.Application.Contracts.Models;

public class ShahkarInquiryBodyData
{
    public string requestId { get; set; }
    public string serviceNumber { get; set; }
    public int serviceType { get; set; }
    public int identificationType { get; set; }
    public string identificationNo { get; set; }
}