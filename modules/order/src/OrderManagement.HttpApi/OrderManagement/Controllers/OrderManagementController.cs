using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Esale.Share.Authorize;
using Microsoft.AspNetCore.Authorization;

namespace OrderManagement.HttpApi;

[RemoteService]
[Route("api/services/app/OrderService/[action]")]
public class OrderManagementController : IOrderAppService
{
    private readonly IOrderAppService _orderAppService;

    public OrderManagementController(IOrderAppService orderAppService)
        => _orderAppService = orderAppService;
    [UserAuthorization]
    [HttpPost]
    public async Task<CustomerOrderDto> CancelOrder(int orderId)
        => await _orderAppService.CancelOrder(orderId);
    [HttpPost]
    public async Task<bool> Test()
    {
        return await _orderAppService.Test();
    }
   
    [HttpPost]
    public async Task CommitOrder(CommitOrderDto commitOrderDto)
        => await _orderAppService.CommitOrder(commitOrderDto);

    [HttpGet]
    [UserAuthorization]
    public async Task<List<CustomerOrderReportDto>> GetCompaniesCustomerOrders()
        => await _orderAppService.GetCompaniesCustomerOrders();

    [HttpGet]
    [UserAuthorization]
    public async Task<List<CustomerOrderPriorityUserDto>> GetCustomerInfoPriorityUser()
        => await _orderAppService.GetCustomerInfoPriorityUser();

    [HttpGet]
    [UserAuthorization]
    public List<CustomerOrder_OrderDetailDto> GetCustomerOrderList()
        => _orderAppService.GetCustomerOrderList();

    [HttpPost]
    [UserAuthorization]
    public async Task InsertUserRejectionAdvocacyPlan(string userSmsCode)
        => await _orderAppService.InsertUserRejectionAdvocacyPlan(userSmsCode);

    [HttpPost]
    [UserAuthorization]
    public async Task<bool> UserRejectionStatus()
        => await _orderAppService.UserRejectionStatus();
}
