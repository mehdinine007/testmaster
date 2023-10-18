using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts;
using Esale.Share.Authorize;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/OrderStatusInquiry/[action]")]
public class OrderStatusInquiryController : Controller // , IOrderStatusInquiryService
{
    private readonly IOrderStatusInquiryService _orderStatusInquiryService;

    public OrderStatusInquiryController(IOrderStatusInquiryService orderStatusInquiryService)
        => _orderStatusInquiryService = orderStatusInquiryService;

    [HttpPost]
    public async Task<OrderStatusInquiryResultDto> GetOrderDeilvery(OrderStatusInquiryCommitDto orderStatusInquiryCommitDto)
        => await _orderStatusInquiryService.GetOrderDeilvery(orderStatusInquiryCommitDto);

    //public Task<OrderStatusInquiryDto> GetCurrentUserOrderStatus(string nationalCode, int customerOrderId)
    //{
    //    throw new NotImplementedException();
    //}
}
