using Grpc.Core;
using OrderManagement.Application.TestService;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class GrpcTestService : TestService.TestService.TestServiceBase
{
    public override Task<ServiceResponse> SampleService(ServiceRequest request, ServerCallContext context)
    {
        return Task.FromResult(new ServiceResponse()
        {
            ResponseMessage = "Service ia accessible"
        });
    }
}
