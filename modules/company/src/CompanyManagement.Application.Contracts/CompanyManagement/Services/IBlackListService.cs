﻿using CompanyManagement.Application.Contracts.CompanyManagement.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services
{
    public interface IBlackListService: IApplicationService
    {
        Task<bool> Inquiry(string nationalCode);
    }
}
