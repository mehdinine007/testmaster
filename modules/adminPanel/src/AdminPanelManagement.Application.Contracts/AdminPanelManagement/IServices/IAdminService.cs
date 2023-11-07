using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices
{
    public interface IAdminService: IApplicationService
    {
        Task<UserInfo_CustomerOrderDto> GetCustomerOrderList(string nationalCode);
    }
}
