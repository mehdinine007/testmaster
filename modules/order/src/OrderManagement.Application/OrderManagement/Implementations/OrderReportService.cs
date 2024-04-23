using Esale.Share.Authorize;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class OrderReportService : ApplicationService,IOrderReportService
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IReportService _reportService;
        public OrderReportService(IOrderAppService orderAppService, IReportService reportService)
        {
            _orderAppService = orderAppService;
            _reportService = reportService;
        }

        [SecuredOperation(OrderAppServicePermissionConstants.GetOrderDetailById)]
        public async Task<string> RptOrderDetail(int orderId, string reportName)
        {
            var orderDetail = await _orderAppService.GetReportOrderDetail(orderId);
            return await _reportService.Execute(reportName, orderDetail);
        }
    }
}
