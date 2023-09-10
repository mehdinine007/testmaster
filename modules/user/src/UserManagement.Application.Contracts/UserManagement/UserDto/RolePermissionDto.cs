using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;

namespace UserManagement.Application.Contracts.UserManagement.UserDto
{
    public class RolePermissionDto
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public List<PermissionDataDto> Permissions { get; set; }
    }
}
