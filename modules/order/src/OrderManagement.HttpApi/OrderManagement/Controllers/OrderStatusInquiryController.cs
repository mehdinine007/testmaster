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
[Route("api/services/app/AgencyService/[action]")]
[UserAuthorization]
public class OrderStatusInquiryController : Controller // , IOrderStatusInquiryService
{
    private readonly IOrderStatusInquiryService _orderStatusInquiryService;

    public OrderStatusInquiryController(IOrderStatusInquiryService orderStatusInquiryService)
        => _orderStatusInquiryService = orderStatusInquiryService;

    [HttpPost]
    [UserAuthorization]
    public async Task<OrderStatusInquiryResultDto> CommitOrderDeilveryLog(OrderStatusInquiryCommitDto orderStatusInquiryCommitDto)
        => await _orderStatusInquiryService.CommitOrderDeilveryLog(orderStatusInquiryCommitDto);

    //public Task<OrderStatusInquiryDto> GetCurrentUserOrderStatus(string nationalCode, int customerOrderId)
    //{
    //    throw new NotImplementedException();
    //}
}
