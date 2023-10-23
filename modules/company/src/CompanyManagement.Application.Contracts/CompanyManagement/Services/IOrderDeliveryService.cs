using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services
{
    public interface IOrderDeliveryService
    {
        Task<OrderDelivaryDto> GetOrderDelivary(string nationalCod, long orderId);
    }
}
