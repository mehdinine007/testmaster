﻿using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Auditing;
using System;
using OrderManagement.Domain.Shared;
using IFG.Core.Utility.Tools;
using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client;
using OrderManagement.Application.Contracts.OrderManagement;

namespace OrderManagement.HttpApi;

[RemoteService]
[Route("api/services/app/OrderService/[action]")]
public class OrderManagementController
{
    private readonly IOrderAppService _orderAppService;

    public OrderManagementController(IOrderAppService orderAppService)
        => _orderAppService = orderAppService;


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

    public async Task<CommitOrderResultDto> CommitOrder([FromBody] CommitOrderDto commitOrderDto)
         => await _orderAppService.CommitOrder(commitOrderDto);
    [DisableAuditing]
    [HttpGet]

    public async Task<List<CustomerOrderReportDto>> GetCompaniesCustomerOrders()
        => await _orderAppService.GetCompaniesCustomerOrders();
    [DisableAuditing]
    [HttpGet]

    public async Task<List<CustomerOrderPriorityUserDto>> GetCustomerInfoPriorityUser()
        => await _orderAppService.GetCustomerInfoPriorityUser();

    [HttpGet]

    public async Task<CustomerOrder_OrderDetailTreeDto> GetCustomerOrderList(string attachmentEntityType, string attachmentlocation)
        => await _orderAppService.GetCustomerOrderList(new CustomerOrderQueryDto { AttachmentType= EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentEntityType),Attachmentlocation= EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation) });

    [RemoteService(IsEnabled = false)]
    public async Task<CustomerOrder_OrderDetailDto> GetOrderDetailById(int id, string attachmentEntityType, string attachmentlocation)
        => await _orderAppService.GetOrderDetailById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentEntityType));

    [RemoteService(IsEnabled = false)]
    public async Task<CustomerOrder_OrderDetailDto> GetSaleDetailByUid(Guid saleDetailUid, string attachmentEntityType, string attachmentlocation)
        => await _orderAppService.GetSaleDetailByUid(saleDetailUid, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentEntityType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));

    [DisableAuditing]
    [HttpPost]

    public async Task<CustomerOrder_OrderDetailDto> GetDetail([FromBody] SaleDetail_Order_InquiryDto inquiryDto)
        => await _orderAppService.GetDetail(inquiryDto);

    [HttpPost]

    public async Task<bool> InsertUserRejectionAdvocacyPlan(string userSmsCode)
    {
        await _orderAppService.InsertUserRejectionAdvocacyPlan(userSmsCode);
        return true;
    }
    [DisableAuditing]
    [HttpPost]

    public async Task<bool> UserRejectionStatus()
        => await _orderAppService.UserRejectionStatus();
    //[HttpPost]
    //
    //public async Task<HandShakeResultDto> PrepareOrderForPayment(int customerOrder, int pspAccountId)
    //    => await _orderAppService.PrepareOrderForPayment(customerOrder, pspAccountId);

    [HttpPost]
    [RemoteService(IsEnabled = false)]
    public async Task CheckoutPayment(IPgCallBackRequest callBackRequest)
        => await _orderAppService.CheckoutPayment(callBackRequest);

    [HttpPost]
    public async Task RetryPaymentForVerify()
        => await _orderAppService.RetryPaymentForVerify();
    [HttpGet]
    public async Task<List<ClientOrderDetailDto>> GetOrderDetailFromOrganizationList()
        => await _orderAppService.GetOrderDetailFromOrganizationList();
}
