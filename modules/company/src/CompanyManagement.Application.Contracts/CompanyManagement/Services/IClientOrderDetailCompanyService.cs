using CompanyManagement.Application.Contracts.CompanyManagement.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services
{
    public interface IClientOrderDetailCompanyService
    {
        Task<bool> Save(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos);
        Task<List<ClientOrderDetailDto>> GetList(string nationalCode);
        Task<OrderDelivaryDto> GetOrderDelivary(string nationalCod, long orderId);
    }
}
