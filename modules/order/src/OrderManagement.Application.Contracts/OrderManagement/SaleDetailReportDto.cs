using System.Collections.Generic;

namespace OrderManagement.Application.Contracts
{
    public class SaleDetailReportDto
    {
        public string PaymentStatus { get; set; }

        public long Count { get; set; }

        public string SaleDetailTitle { get; set; }

        public string AgencyName { get; set; }
    }

    public class SaleDetailResultDto
    {
        public int SaleDetailId { get; set; }

        public List<SaleDetailReportDto> Reports { get; set; }
    }
}