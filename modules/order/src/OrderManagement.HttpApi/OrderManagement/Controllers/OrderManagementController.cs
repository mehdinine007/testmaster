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
    public async Task<bool> CommitOrder(CommitOrderDto commitOrderDto)
    {
        await _orderAppService.CommitOrder(commitOrderDto);
        return true;
    }

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
    public async Task<bool> InsertUserRejectionAdvocacyPlan(string userSmsCode)
    {
        await _orderAppService.InsertUserRejectionAdvocacyPlan(userSmsCode);
        return true;
    }

    [HttpPost]
    [UserAuthorization]
    public async Task<bool> UserRejectionStatus()
        => await _orderAppService.UserRejectionStatus();
    [HttpPost]
    public bool TestNohi()
    {
        return true;
    }

    
}
