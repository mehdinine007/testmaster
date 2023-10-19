using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Auditing;
using System;
using OrderManagement.Domain.Shared;
using Esale.Core.Utility.Tools;
using Esale.Core.Bases;
using System.Linq;
using System.Reflection;

namespace OrderManagement.HttpApi;

[RemoteService]
[Route("api/services/app/OrderService/[action]")]
public class OrderManagementController
{
    private readonly IOrderAppService _orderAppService;

    //public OrderManagementController(IOrderAppService orderAppService)
    //    => _orderAppService = orderAppService;


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

    public Task<List<CustomerOrder_OrderDetailDto>> GetCustomerOrderList(string attachmentEntityType)
        => _orderAppService.GetCustomerOrderList(EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentEntityType));

    [RemoteService(IsEnabled = false)]
    public async Task<CustomerOrder_OrderDetailDto> GetOrderDetailById(int id, string attachmentEntityType)
        => await _orderAppService.GetOrderDetailById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentEntityType));

    [RemoteService(IsEnabled = false)]
    public async Task<CustomerOrder_OrderDetailDto> GetSaleDetailByUid(Guid saleDetailUid, string attachmentEntityType)
        => await _orderAppService.GetSaleDetailByUid(saleDetailUid, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentEntityType));

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
    public string TTTT()
    {
        var baseType = typeof(BasePermissionConstants);
        var appDomain = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => baseType.IsAssignableFrom(x))
            .ToList();
        List<(string permission, List<KeyValuePair<string, string>> fieldInfo)> fields = new();
        foreach (var item in appDomain)
        {
            var permissionName = item.Name;
            var fieldsInfo = item.GetRuntimeFields().Select(x => new KeyValuePair<string, string>(x.Name,x.GetValue(x).ToString())).ToList();
            fields.Add(new()
            {
                permission = permissionName,
                fieldInfo = fieldsInfo
            });
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(fields);
        //return new JsonResult(appDomain.Select(x => new
        //{
        //    name = x.Name
        //}).ToList());
    }

}
