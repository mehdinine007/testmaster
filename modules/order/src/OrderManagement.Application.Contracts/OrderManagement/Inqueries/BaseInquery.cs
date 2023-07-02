using Volo.Abp.Application.Dtos;

namespace OrderManagement.Application.Contracts.OrderManagement.Inqueries
{
    public class BaseInquery : PagedResultRequestDto
    {
    }

    public class SaleDetailReportInquery : BaseInquery
    {
        public bool ActiveOrDeactive { get; set; }
    }
}
