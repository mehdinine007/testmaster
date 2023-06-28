namespace OrderManagement.Application.Contracts
{
    public class SaleDetailReportDto
    {
        public string PaymentStatus { get; set; }

        public int Count { get; set; }

        public string SaleDetailTitle { get; set; }

        public string AgencyName { get; set; }
    }
}