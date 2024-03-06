using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ICompanyGrpcClient : IApplicationService
    {
        Task<ClientOrderDeliveryInformationDto> ValidateClientOrderDeliveryDate(ClientOrderDeliveryInformationRequestDto clientOrderRequest);
        Task<List<ClientOrderDetailDto>> GetOrderDetailList(string nationalCode);
    }
}
