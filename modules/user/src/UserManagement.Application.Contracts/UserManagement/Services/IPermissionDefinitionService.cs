using UserManagement.Application.Contracts.Models;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IPermissionDefinitionService : IApplicationService
    {
        Task InsertList();
        Task<List<PermissionDefinitionDto>> GetList();

        Task<List<PermissionDefinitionDto>> Insert(PermissionDefinitionDto permission);
    }
}
