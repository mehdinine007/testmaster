using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IRandomGenerator : IApplicationService
    {
        int GetUniqueInt();
    }
}
