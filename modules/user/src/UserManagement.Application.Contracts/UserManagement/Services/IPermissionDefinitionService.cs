using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
