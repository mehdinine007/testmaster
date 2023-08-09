using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IOrderStatusInquiryService : IApplicationService
    {
        Task<OrderStatusInquiryDto> Insert(OrderStatusInquiryDto orderStatusInquiryDto);
    }
}
