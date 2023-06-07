using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IOrderAppService : IApplicationService
    {
        List<CustomerOrder_OrderDetailDto> GetCustomerOrderList();

        Task<CustomerOrderDto> CancelOrder(int orderId);

        Task InsertUserRejectionAdvocacyPlan(string userSmsCode);
        Task UpdateStatus(CustomerOrderDto customerOrderDto);
        Task<List<CustomerOrderReportDto>> GetCompaniesCustomerOrders();

        Task<List<CustomerOrderPriorityUserDto>> GetCustomerInfoPriorityUser();

        Task<CommitOrderResultDto> CommitOrder(CommitOrderDto commitOrderDto);
        Task RetryPaymentForVerify();
        Task<bool> UserRejectionStatus();

        Task<bool> Test();

        Task<IPaymentResult> CheckoutPayment(IPgCallBackRequest callBackRequest);
    }
}
