using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IOrderAppService : IApplicationService
    {
        Task<GetOrderByIdResponseDto> GetOrderById(int orderId);

        Task<CustomerOrder_OrderDetailTreeDto> GetCustomerOrderList(List<AttachmentEntityTypeEnum>? attachmentType = null, List<AttachmentLocationEnum>? attachmentlocation = null);

        Task<CustomerOrder_OrderDetailDto> GetOrderDetailById(int id, List<AttachmentEntityTypeEnum>? attachmentType = null, List<AttachmentLocationEnum>? attachmentlocation = null);

        Task<CustomerOrder_OrderDetailDto> GetSaleDetailByUid(Guid saleDetailUid, List<AttachmentEntityTypeEnum>? attachmentType = null, List<AttachmentLocationEnum>? attachmentlocation = null);

        Task<CustomerOrder_OrderDetailDto> GetDetail(SaleDetail_Order_InquiryDto inquiryDto);

        Task<CustomerOrderDto> CancelOrder(int orderId);

        Task InsertUserRejectionAdvocacyPlan(string userSmsCode);
        Task UpdateStatus(CustomerOrderDto customerOrderDto);
        Task UpdateSignStatus(CustomerOrderDto customerOrderDto);
        Task<List<CustomerOrderReportDto>> GetCompaniesCustomerOrders();

        Task<List<CustomerOrderPriorityUserDto>> GetCustomerInfoPriorityUser();

        Task<CommitOrderResultDto> CommitOrder(CommitOrderDto commitOrderDto);
        Task RetryPaymentForVerify();
        Task RetryOrderForVerify();
        Task<bool> UserRejectionStatus();

        Task<bool> Test();

        Task<IPaymentResult> CheckoutPayment(IPgCallBackRequest callBackRequest);

        Task<bool> NationalCodeExistsInPriority(string nationalCode);
        Task<List<ClientOrderDetailDto>> GetOrderDetailFromOrganizationList();

        Task<OrderDetailDto> GetReportOrderDetail(int id);
    }
}
