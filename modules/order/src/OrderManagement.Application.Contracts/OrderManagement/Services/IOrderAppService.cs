using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IOrderAppService : IApplicationService
    {
        Task<List<CustomerOrder_OrderDetailDto>> GetCustomerOrderList(AttachmentEntityTypeEnum attachmentType);

        Task<CustomerOrder_OrderDetailDto> GetOrderDetailById(int id, AttachmentEntityTypeEnum attachmentType);

        Task<CustomerOrder_OrderDetailDto> GetSaleDetailByUid(Guid saleDetailUid, AttachmentEntityTypeEnum attachmentType);

        Task<CustomerOrder_OrderDetailDto> GetDetail(SaleDetail_Order_InquiryDto inquiryDto);

        Task<CustomerOrderDto> CancelOrder(int orderId);

        Task InsertUserRejectionAdvocacyPlan(string userSmsCode);
        Task UpdateStatus(CustomerOrderDto customerOrderDto);
        Task<List<CustomerOrderReportDto>> GetCompaniesCustomerOrders();

        Task<List<CustomerOrderPriorityUserDto>> GetCustomerInfoPriorityUser();

        Task<CommitOrderResultDto> CommitOrder(CommitOrderDto commitOrderDto);
        Task RetryPaymentForVerify();
        Task RetryOrderForVerify();
        Task<bool> UserRejectionStatus();

        Task<bool> Test();

        Task<IPaymentResult> CheckoutPayment(IPgCallBackRequest callBackRequest);
    }
}
