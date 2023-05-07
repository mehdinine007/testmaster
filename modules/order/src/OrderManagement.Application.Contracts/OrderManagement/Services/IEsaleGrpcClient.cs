using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IEsaleGrpcClient : IApplicationService
    {
        UserDto GetUserById(long userId);
    }
}
