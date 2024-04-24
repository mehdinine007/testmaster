using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts;
using FastReport.Web;
using System.IO;
using System.Collections.Generic;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/OrderReportService/[action]")]
//[UserAuthorization]
public class OrderReportController : Controller
{
    private readonly IOrderReportService _orderReportService;
    private readonly IReportService _reportService;
    public OrderReportController(IOrderReportService orderReportService, IReportService reportService)
    {
        _orderReportService = orderReportService;
        _reportService = reportService;
    }

    [HttpGet]
    public Task<string> RptContractForm(int orderId)
    => _orderReportService.RptOrderDetail(orderId, OrderConstant.ContractReportName);

    [HttpGet]
    public Task<string> RptFactor(int orderId)
    => _orderReportService.RptOrderDetail(orderId, OrderConstant.FactorReportName);

    //[HttpGet]
    //public Task<string> ReportTest()
    //{
    //    return _reportService.Execute("RptContractForm", new Dictionary<string, object>());
    //}

}
