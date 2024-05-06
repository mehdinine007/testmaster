using Esale.Share.Authorize;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using Permission.Order;
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
            var orderDetails = new List<OrderDetailDto>();
            orderDetails.Add(orderDetail);
            return await _reportService.Execute(reportName,orderDetails);
        }
    }
}
