using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IRandomGenerator : IApplicationService
    {
        int GetUniqueInt();
    }
}
