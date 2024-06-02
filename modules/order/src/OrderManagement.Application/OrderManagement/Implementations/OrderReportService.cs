using Esale.Share.Authorize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using Permission.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class OrderReportService : ApplicationService, IOrderReportService
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IReportService _reportService;
        private readonly IConfiguration _configuration;
        public OrderReportService(IOrderAppService orderAppService,
                                  IReportService reportService,
                                  IConfiguration configuration
            )
        {
            _orderAppService = orderAppService;
            _reportService = reportService;
            _configuration = configuration;
        }

        [SecuredOperation(OrderAppServicePermissionConstants.GetOrderDetailById)]
        public async Task<string> RptOrderDetail(int orderId, string reportName)
        {
            var organizationPrefix = _configuration.GetValue<string>("ReportOrganizationPrefix")
                ?? throw new UserFriendlyException(OrderConstant.OrgnizationPrefixNotFound, code: OrderConstant.OrgnizationPrefixNotFoundId);
            var orderDetail = await _orderAppService.GetReportOrderDetail(orderId);
            var orderDetails = new List<OrderDetailDto>();
            orderDetails.Add(orderDetail);
            return await _reportService.Execute(reportName + "_" + organizationPrefix, orderDetails);
        }
    }
}
