using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OrderManagement.HttpApi;

public class OrderManagementController : IOrderAppService
{
    private readonly IOrderAppService _orderAppService;

    public OrderManagementController(IOrderAppService orderAppService)
        => _orderAppService = orderAppService;

    public async Task<CustomerOrderDto> CancelOrder(int orderId)
        => await _orderAppService.CancelOrder(orderId);

    public async Task<List<CustomerOrderReportDto>> GetCompaniesCustomerOrders()
        => await _orderAppService.GetCompaniesCustomerOrders();

    public async Task<List<CustomerOrderPriorityUserDto>> GetCustomerInfoPriorityUser()
        => await _orderAppService.GetCustomerInfoPriorityUser();

    public List<CustomerOrder_OrderDetailDto> GetCustomerOrderList()
        => _orderAppService.GetCustomerOrderList();

    public async Task InsertUserRejectionAdvocacyPlan(string userSmsCode)
        => await _orderAppService.InsertUserRejectionAdvocacyPlan(userSmsCode);
}