using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IGetOrderDetailService: IApplicationService
    {
        Task<List<ClientOrderDetailDto>> GetOrderDetailGetList(string nationalCode);
    }
}
