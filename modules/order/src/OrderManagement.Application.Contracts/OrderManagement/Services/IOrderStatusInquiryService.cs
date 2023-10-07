using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IOrderStatusInquiryService : IApplicationService
    {
        Task<OrderStatusInquiryResultDto> GetOrderDeilvery(OrderStatusInquiryCommitDto orderStatusInquiryCommitDto);

        Task<OrderStatusInquiryDto> GetCurrentUserOrderStatus(string nationalCode,int customerOrderId);
    }
}
