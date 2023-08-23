using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Esale.Share.Authorize;
using Volo.Abp.Auditing;
using System;
using OrderManagement.Domain.Shared;

namespace OrderManagement.HttpApi;

[RemoteService]
[Route("api/services/app/OrderService/[action]")]
public class OrderManagementController
{
    private readonly IOrderAppService _orderAppService;

    public OrderManagementController(IOrderAppService orderAppService)
        => _orderAppService = orderAppService;
    [UserAuthorization]
    [HttpPost]
    public async Task<bool> CancelOrder(int orderId)
    {
        await _orderAppService.CancelOrder(orderId);
        return true;
    }
    [HttpPost]
    public async Task<bool> Test()
    {
        await _orderAppService.Test();
        return true;
    }

    [HttpPost]
    [UserAuthorization]
    public async Task<CommitOrderResultDto> CommitOrder([FromBody] CommitOrderDto commitOrderDto)
         => await _orderAppService.CommitOrder(commitOrderDto);
    [DisableAuditing]
    [HttpGet]
    [UserAuthorization]
    public async Task<List<CustomerOrderReportDto>> GetCompaniesCustomerOrders()
        => await _orderAppService.GetCompaniesCustomerOrders();
    [DisableAuditing]
    [HttpGet]
    [UserAuthorization]
    public async Task<List<CustomerOrderPriorityUserDto>> GetCustomerInfoPriorityUser()
        => await _orderAppService.GetCustomerInfoPriorityUser();
    
    [HttpGet]

    public Task<List<CustomerOrder_OrderDetailDto>> GetCustomerOrderList(AttachmentEntityTypeEnum attachmentEntityType)
        => _orderAppService.GetCustomerOrderList(attachmentEntityType);

    [RemoteService(IsEnabled = false)]
    public async Task<CustomerOrder_OrderDetailDto> GetOrderDetailById(int id, AttachmentEntityTypeEnum attachmentEntityType)
        => await _orderAppService.GetOrderDetailById(id, attachmentEntityType);

    [RemoteService(IsEnabled =false)]
    public async Task<CustomerOrder_OrderDetailDto> GetSaleDetailByUid(Guid saleDetailUid, AttachmentEntityTypeEnum attachmentEntityType)
        => await _orderAppService.GetSaleDetailByUid(saleDetailUid, attachmentEntityType);

    [DisableAuditing]
    [HttpPost]
    [UserAuthorization]
    public async Task<CustomerOrder_OrderDetailDto> GetDetail([FromBody]SaleDetail_Order_InquiryDto inquiryDto)
        => await _orderAppService.GetDetail(inquiryDto);

    [HttpPost]
    [UserAuthorization]
    public async Task<bool> InsertUserRejectionAdvocacyPlan(string userSmsCode)
    {
        await _orderAppService.InsertUserRejectionAdvocacyPlan(userSmsCode);
        return true;
    }
    [DisableAuditing]
    [HttpPost]
    [UserAuthorization]
    public async Task<bool> UserRejectionStatus()
        => await _orderAppService.UserRejectionStatus();
    //[HttpPost]
    //[UserAuthorization]
    //public async Task<HandShakeResultDto> PrepareOrderForPayment(int customerOrder, int pspAccountId)
    //    => await _orderAppService.PrepareOrderForPayment(customerOrder, pspAccountId);

    [HttpPost]
    [RemoteService(IsEnabled = false)]
    public async Task CheckoutPayment(IPgCallBackRequest callBackRequest)
        => await _orderAppService.CheckoutPayment(callBackRequest);

    [HttpPost]
    public async Task RetryPaymentForVerify()
        => await _orderAppService.RetryPaymentForVerify();

}
