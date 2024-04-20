using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client.Sign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ISignGrpcClient: IApplicationService
    {
        Task<CreateSignGrpcClientResponse> CreateSign(CreateSignGrpcClientRequest createSignGrpcClientRequest);
    }
}
