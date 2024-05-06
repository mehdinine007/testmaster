using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class GetOrderDetailService : ApplicationService, IGetOrderDetailService
    {
        private readonly ICompanyGrpcClient _companyGrpcClient;
        public GetOrderDetailService(ICompanyGrpcClient companyGrpcClient)
        {
            _companyGrpcClient = companyGrpcClient;
        }


        public async Task<List<ClientOrderDetailDto>> GetOrderDetailList(string nationalCode)
        {
            var orderDetails = await _companyGrpcClient.GetOrderDetailList();
            return orderDetails;
        }
    }
}
