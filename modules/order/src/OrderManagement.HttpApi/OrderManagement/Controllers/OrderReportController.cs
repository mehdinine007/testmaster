using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/OrderReportService/[action]")]
//[UserAuthorization]
public class OrderReportController : Controller
{
    private readonly IOrderReportService _orderReportService;
    public OrderReportController(IOrderReportService orderReportService)
        => _orderReportService = orderReportService;

    [HttpGet]
    public Task<string> RptContractForm(int orderId)
    => _orderReportService.RptOrderDetail(orderId, "RptContractForm");
}
