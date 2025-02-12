﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts
{
    public interface IOrderReportService : IApplicationService
    {
        Task<string> RptOrderDetail(int orderId,string reportName);
    }
}
